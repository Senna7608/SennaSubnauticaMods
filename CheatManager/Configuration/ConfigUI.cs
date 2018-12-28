using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

namespace CheatManager.Configuration
{
    internal class ConfigUI : MonoBehaviour
    {
        public static ConfigUI Instance { get; private set; }
        private Rect windowRect;
        private static bool initStyles = false;
        private static int selected = -1;
        private Event keyEvent;
        private KeyCode newKey;
        private static bool waitingForKey = false;
        private List<string> HotkeyLabels = new List<string>();
        private List<string> HotkeyButtons = new List<string>();
        private List<string> SettingLabels = new List<string>();

        private List<GUIHelper.ButtonInfo> buttonInfos = new List<GUIHelper.ButtonInfo>();
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

            GUIHelper.CreateButtonsGroup(HotkeyButtons.ToArray(), GUIHelper.BUTTONTYPE.NORMAL_CENTER, ref buttonInfos);            
        }

        public void OnGUI()
        {
            if (!initStyles)
                initStyles = GUIHelper.InitGUIStyles();

            windowRect = GUIHelper.CreatePopupWindow(new Rect(0, 0, 310, 240), "CheatManager: Configuration interface", false, true);

            GUI.FocusControl("CheatManager.ConfigUI");

            GUIHelper.CreateItemsGrid(new Rect(windowRect.x, windowRect.y, windowRect.width / 2, windowRect.height), space, 1, HotkeyLabels, GUIHelper.GUI_ITEM.TEXTFIELD);            

            int sBtn = GUIHelper.CreateButtonsGrid(new Rect(windowRect.x + windowRect.width / 2, windowRect.y, windowRect.width / 2, windowRect.height), space, 1, buttonInfos, out float lastY);
                        
            if (sBtn != -1)
            {
                StartAssignment(HotkeyButtons[sBtn]);
                selected = sBtn;
                buttonInfos[sBtn].Name = "Press any key!";
            }           
            
            if (GUI.Button(new Rect(windowRect.x + space, lastY + space * 2, windowRect.width / 2 - space * 2, 40), "Save"))
            {                
                SaveAndExit();
            }
            else if (GUI.Button(new Rect(windowRect.x + space + windowRect.width / 2, lastY + space * 2, windowRect.width / 2 - space * 2, 40), "Cancel"))
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
                buttonInfos[isFirst].Name = HotkeyButtons[selected];
            }

            HotkeyButtons[selected] = newKey.ToString();
            buttonInfos[selected].Name = HotkeyButtons[selected];
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
