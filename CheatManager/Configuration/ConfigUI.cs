using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.GUIHelper;

namespace CheatManager.Configuration
{
    internal class ConfigUI : MonoBehaviour
    {
        public static ConfigUI Instance { get; private set; }
        private Rect windowRect = new Rect(0, 0, 310, 240);
        private Rect drawRect;
        private List<Rect> buttonsRect;       
        private static int selected = -1;
        private Event keyEvent;
        private KeyCode newKey;
        private static bool waitingForKey = false;
        private List<string> HotkeyLabels = new List<string>();
        private List<string> HotkeyButtons = new List<string>();
        private List<string> SettingLabels = new List<string>();
        private List<GuiItem> buttonInfo = new List<GuiItem>();
        private List<GuiItem> itemInfo = new List<GuiItem>();

        private static readonly float space = 10f;

        public void Awake()
        {
            useGUILayout = false;
            Instance = this;
            InitItems();
        }

        private void InitItems()
        {
            foreach (KeyValuePair<string, string> key in CmConfig.Section_hotkeys)
            {
                HotkeyLabels.Add(key.Key);
                HotkeyButtons.Add(key.Value);
            }

            foreach (KeyValuePair<string, string> key in CmConfig.Section_settings)
            {
                SettingLabels.Add(key.Key);                
            }

            drawRect = SNWindow.InitWindowRect(windowRect, true);

            List<Rect> itemsRect = new Rect(drawRect.x, drawRect.y, drawRect.width / 2, drawRect.height).SetGridItemsRect(1, HotkeyLabels.Count, Screen.height / 45, 10, 10);

            buttonsRect = new Rect(drawRect.x + drawRect.width / 2, drawRect.y, drawRect.width / 2, drawRect.height).SetGridItemsRect(1, HotkeyButtons.Count + 1, Screen.height / 45, 10, 10);

            itemInfo.CreateGuiItemsGroup(HotkeyLabels.ToArray(), itemsRect, GuiItemType.TEXTFIELD, new GuiItemColor());

            buttonInfo.CreateGuiItemsGroup(HotkeyButtons.ToArray(), buttonsRect, GuiItemType.NORMALBUTTON, new GuiItemColor(GuiColor.Gray, GuiColor.White, GuiColor.Green), null, GuiItemState.NORMAL, true, FontStyle.Bold, TextAnchor.MiddleCenter);
        }

        public void OnGUI()
        {
            SNWindow.CreateWindow(windowRect, "CheatManager: Configuration interface", false, true);

            GUI.FocusControl("CheatManager.ConfigUI");

            itemInfo.DrawGuiItemsGroup();

            GuiItemEvent sBtn = buttonInfo.DrawGuiItemsGroup();

            if (sBtn.ItemID != -1)
            {
                StartAssignment(HotkeyButtons[sBtn.ItemID]);
                selected = sBtn.ItemID;
                buttonInfo[sBtn.ItemID].Name = "Press any key!";
            }           
            
            if (GUI.Button(new Rect(drawRect.x + space, buttonsRect.GetLast().y + space * 2, drawRect.width / 2 - space * 2, 40), "Save"))
            {                
                SaveAndExit();
            }
            else if (GUI.Button(new Rect(drawRect.x + space + drawRect.width / 2, buttonsRect.GetLast().y + space * 2, drawRect.width / 2 - space * 2, 40), "Cancel"))
            {
                Destroy(Instance);
            }
            
            keyEvent = Event.current;

            if (keyEvent.isKey && waitingForKey)
            {
                newKey = keyEvent.keyCode;
                waitingForKey = false;
            }
        }        

        public void StartAssignment(object keyName)
        {
            if (!waitingForKey)
                StartCoroutine(AssignKey(keyName));
        }

        private IEnumerator AssignKey(object keyName)
        {
            waitingForKey = true;

            yield return WaitForKey();

            int isFirst = 0;            
            int keyCount = 0;

            for(int i = 0; i < HotkeyButtons.Count; i++)
            {
                if (HotkeyButtons[i].Equals(newKey.ToString()))
                {
                    if (keyCount == 0)
                        isFirst = i;

                    keyCount++;                                        
                }                
            }

            if (keyCount > 0 && isFirst != selected)
            {
                SNLogger.Log("CheatManager", "Error! Duplicate keybind found, swapping keys...");
                HotkeyButtons[isFirst] = HotkeyButtons[selected];
                buttonInfo[isFirst].Name = HotkeyButtons[selected];
            }

            HotkeyButtons[selected] = newKey.ToString();
            buttonInfo[selected].Name = HotkeyButtons[selected];
            selected = -1;            
            yield return null;
        }

        private IEnumerator WaitForKey()
        {
            while (!keyEvent.isKey)
                yield return null;
        }

        private void SaveAndExit()
        {
            for (int i = 0; i < HotkeyLabels.Count; i++)
            {
                CmConfig.Section_hotkeys[HotkeyLabels[i]] = HotkeyButtons[i];
            }

            CmConfig.WriteConfig();
            CmConfig.SetKeyBindings();            
            Destroy(Instance);
        }

        internal static void InitWindow()
        {
            Instance = Load();                    
        }

        public static ConfigUI Load()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(ConfigUI)) as ConfigUI;

                if (Instance == null)
                {
                    GameObject go = new GameObject().AddComponent<ConfigUI>().gameObject;
                    go.name = "CheatManager.ConfigUI";
                    Instance = go.GetComponent<ConfigUI>();
                }
            }

            return Instance;
        }
    }    
}
