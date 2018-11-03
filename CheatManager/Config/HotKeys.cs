using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

namespace CheatManager.Config
{
    internal class HotKeys : MonoBehaviour
    {
        public static HotKeys Instance { get; private set; }
        private Rect windowRect;
        private static bool initStyles = false;
        private static int selected = -1;
        private Event keyEvent;
        private KeyCode newKey;
        private static bool waitingForKey = false;
        private List<string> hotkeyLabels = new List<string>();
        private List<string> hotkeyButtons = new List<string>();
        private List<GUIHelper.ButtonInfo> buttonInfos = new List<GUIHelper.ButtonInfo>();

        public void Awake()
        {
            useGUILayout = false;
            Instance = this;
            InitItems();
        }

        private void InitItems()
        {
            foreach (KeyValuePair<string, string> key in Config.HotKeys)
            {
                hotkeyLabels.Add(key.Key);
                hotkeyButtons.Add(key.Value);
            }

            GUIHelper.CreateButtonsList(hotkeyButtons.ToArray(), GUIHelper.BUTTONTYPE.NORMAL_CENTER, ref buttonInfos);
        }

        public void OnGUI()
        {
            if (!initStyles)
                initStyles = GUIHelper.SetCustomStyles();

            windowRect = GUIHelper.CreatePopupWindow(new Rect(0, 0, 310, 200), "CheatManager: Hotkey Settings", false, true);

            GUI.FocusControl("CheatManager.HotKeys");

            GUIHelper.CreateItemsGrid(new Rect(windowRect.x, windowRect.y, 150, windowRect.height), 10, 1, hotkeyLabels, GUIHelper.GUI_ITEM.TEXTFIELD);            

            int sBtn = GUIHelper.CreateButtonsGrid(new Rect(windowRect.x + 160, windowRect.y, 150, windowRect.height), 10, 1, buttonInfos, out float lastY);
                       
            if (sBtn != -1)
            {
                StartAssignment(hotkeyButtons[sBtn]);
                selected = sBtn;
                buttonInfos[sBtn].Name = "Press any key!";
            }           
            
            if (GUI.Button(new Rect(windowRect.x + 5, lastY + 20, 142.5f, 40), "Save & Close"))
            {                
                SaveAndExit();
            }
            else if (GUI.Button(new Rect(windowRect.x + 152.5f, lastY + 20, 142.5f, 40), "Cancel"))
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

            hotkeyButtons[selected] = newKey.ToString();
            buttonInfos[selected].Name = hotkeyButtons[selected];
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
            for (int i = 0; i < hotkeyLabels.Count; i++)
            {
                Config.HotKeys[hotkeyLabels[i]] = hotkeyButtons[i];
            }

            Config.WriteConfig();
            Config.SetKeyBindings();
            Destroy(Instance);
        }

        internal static void InitWindow()
        {
            Instance = Load();                    
        }

        public static HotKeys Load()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(HotKeys)) as HotKeys;

                if (Instance == null)
                {
                    GameObject hotkeys = new GameObject().AddComponent<HotKeys>().gameObject;
                    hotkeys.name = "CheatManager.HotKeys";
                    Instance = hotkeys.GetComponent<HotKeys>();
                }
            }

            return Instance;
        }
    }    
}
