using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common.GUIHelper;

namespace CheatManager.Configuration
{
    internal class ConfigUI : MonoBehaviour
    {
        public static ConfigUI Instance { get; private set; }
        private Rect windowRect;
        private Rect[] buttonsRect;
        private Rect[] itemsRect;
        private static bool initStyles = false;
        private static int selected = -1;
        private Event keyEvent;
        private KeyCode newKey;
        private static bool waitingForKey = false;
        private List<string> HotkeyLabels = new List<string>();
        private List<string> HotkeyButtons = new List<string>();
        private List<string> SettingLabels = new List<string>();

        private List<SNGUI.GuiItem> buttonInfo = new List<SNGUI.GuiItem>();
        private List<SNGUI.GuiItem> itemInfo = new List<SNGUI.GuiItem>();

        private static readonly float space = 10f;

        public void Awake()
        {
            useGUILayout = false;
            Instance = this;
            InitItems();
        }

        private void InitItems()
        {
            foreach (KeyValuePair<string, string> key in Config.Section_hotkeys)
            {
                HotkeyLabels.Add(key.Key);
                HotkeyButtons.Add(key.Value);
            }

            foreach (KeyValuePair<string, string> key in Config.Section_settings)
            {
                SettingLabels.Add(key.Key);                
            }

            windowRect = SNWindow.InitWindowRect(new Rect(0, 0, 310, 240), true);

            itemsRect = SNWindow.SetGridItemsRect(new Rect(windowRect.x, windowRect.y, windowRect.width / 2, windowRect.height),
                                                1, HotkeyLabels.Count, 10, Screen.height / 45);

            buttonsRect = SNWindow.SetGridItemsRect(new Rect(windowRect.x + windowRect.width / 2, windowRect.y, windowRect.width / 2, windowRect.height),
                                                  1, HotkeyButtons.Count + 1, 10, Screen.height / 45);

            SNGUI.CreateGuiItemsGroup(HotkeyLabels.ToArray(), itemsRect, SNGUI.GuiItemType.TEXTFIELD,
                                      ref itemInfo, Color.green, Color.green, true, FontStyle.Bold, TextAnchor.MiddleRight);

            SNGUI.CreateGuiItemsGroup(HotkeyButtons.ToArray(), buttonsRect, SNGUI.GuiItemType.NORMALBUTTON,
                                      ref buttonInfo, Color.white, Color.green, true, FontStyle.Bold, TextAnchor.MiddleCenter);
        }

        public void OnGUI()
        {
            if (!initStyles)
                initStyles = SNStyles.InitGUIStyles();

            SNWindow.CreateWindow(new Rect(0, 0, 310, 240), "CheatManager: Configuration interface", false, true);

            GUI.FocusControl("CheatManager.ConfigUI");

            SNGUI.DrawGuiItemsGroup(ref itemInfo);

            int sBtn = SNGUI.DrawGuiItemsGroup(ref buttonInfo);

            if (sBtn != -1)
            {
                StartAssignment(HotkeyButtons[sBtn]);
                selected = sBtn;
                buttonInfo[sBtn].Name = "Press any key!";
            }           
            
            if (GUI.Button(new Rect(windowRect.x + space, buttonsRect.GetLast().y + space * 2, windowRect.width / 2 - space * 2, 40), "Save"))
            {                
                SaveAndExit();
            }
            else if (GUI.Button(new Rect(windowRect.x + space + windowRect.width / 2, buttonsRect.GetLast().y + space * 2, windowRect.width / 2 - space * 2, 40), "Cancel"))
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
                Debug.Log("[CheatManager] Error! Duplicate keybind found, swapping keys...");
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
                Config.Section_hotkeys[HotkeyLabels[i]] = HotkeyButtons[i];
            }

            Config.WriteConfig();
            Config.SetKeyBindings();
            Main.Instance.UpdateTitle();
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
