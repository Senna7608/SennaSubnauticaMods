using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

namespace SlotExtender.Config
{
    internal class ConfigUI : MonoBehaviour
    {
        public static ConfigUI Instance { get; private set; }
        private Rect windowRect;
        private static bool initStyles = false;
        private static int selected = -1;
        private Event keyEvent;
        private string newKey;
        private static bool waitingForKey = false;
        private List<string> hotkeyLabels = new List<string>();
        private List<string> hotkeyButtons = new List<string>();
        private List<GUIHelper.ButtonInfo> buttonInfo = new List<GUIHelper.ButtonInfo>();
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
                hotkeyLabels.Add(key.Key);
                hotkeyButtons.Add(key.Value);
            }

            GUIHelper.CreateButtonsGroup(hotkeyButtons.ToArray(), GUIHelper.BUTTONTYPE.NORMAL_CENTER, ref buttonInfo);
        }

        public void OnGUI()
        {
            if (!initStyles)
                initStyles = GUIHelper.InitGUIStyles();

            windowRect = GUIHelper.CreatePopupWindow(new Rect(0, 0, Screen.width / 6, Screen.height / 2.9f), "SlotExtender Configuration Window", false, false);

            GUI.FocusControl("SlotExtender.Bindings");

            GUIHelper.CreateItemsGrid(new Rect(windowRect.x, windowRect.y, windowRect.width / 2, windowRect.height), space, 1, hotkeyLabels, GUIHelper.GUI_ITEM.TEXTFIELD);

            int sBtn = GUIHelper.CreateButtonsGrid(new Rect(windowRect.x + windowRect.width / 2, windowRect.y, windowRect.width / 2, windowRect.height), space, 1, buttonInfo, out float lastY);

            if (sBtn != -1)
            {
                StartAssignment(hotkeyButtons[sBtn]);
                selected = sBtn;
                buttonInfo[sBtn].Name = "Press any key!";
            }

            if (GUI.Button(new Rect(windowRect.x + space, lastY + space * 2, windowRect.width / 2 - space * 2, Screen.height / 22.5f), "Save"))
            {
                SaveAndExit();
            }
            else if (GUI.Button(new Rect(windowRect.x + space + windowRect.width / 2, lastY + space * 2, windowRect.width / 2 - space * 2, Screen.height / 22.5f), "Cancel"))
            {
                Destroy(Instance);
            }

            keyEvent = Event.current;

            if (keyEvent.isKey && waitingForKey)
            {
                newKey = InputHelper.GetKeyCodeAsInputName(keyEvent.keyCode);
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

            for (int i = 0; i < hotkeyButtons.Count; i++)
            {
                if (hotkeyButtons[i].Equals(newKey))
                {
                    if (keyCount == 0)
                        isFirst = i;

                    keyCount++;
                }
            }

            if (keyCount > 0 && isFirst != selected)
            {
                Debug.Log("[SlotExtender] Error! Duplicate keybind found, swapping keys...");
                hotkeyButtons[isFirst] = hotkeyButtons[selected];
                buttonInfo[isFirst].Name = hotkeyButtons[selected];
            }

            hotkeyButtons[selected] = newKey;
            buttonInfo[selected].Name = hotkeyButtons[selected];
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
                Config.Section_hotkeys[hotkeyLabels[i]] = hotkeyButtons[i];
            }

            Config.WriteConfig();
            Config.SetKeyBindings();
            Main.GameInput_OnBindingsChanged();
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
                    go.name = "SlotExtender.ConfigUI";
                    Instance = go.GetComponent<ConfigUI>();
                }
            }

            return Instance;
        }
    }    
}
