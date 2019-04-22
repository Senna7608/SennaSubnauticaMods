using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common.GUIHelper;
using Common;

namespace QuickSlotExtender.Configuration
{
    internal class ConfigUI : MonoBehaviour
    {
        internal ConfigUI Instance { get; private set; }

        private Rect windowRect, drawrect;
        List<Rect> buttonRects;
       
        private int selected = -1;
        private Event keyEvent;
        private string newKey;
        private bool waitingForKey = false;
        private List<string> labels = new List<string>();
        private List<string> buttons = new List<string>();
        private List<GuiItem> buttonInfo = new List<GuiItem>();
        private List<GuiItem> labelInfo = new List<GuiItem>();
        private readonly float space = 10f;

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
                labels.Add(key.Key);
                buttons.Add(key.Value);
            }

            windowRect = new Rect(0, 0, Screen.width / 6, buttons.Count * 48);
            drawrect = SNWindow.InitWindowRect(windowRect);

            List<Rect> labelRects = SNWindow.SetGridItemsRect(new Rect(drawrect.x, drawrect.y, drawrect.width / 2, drawrect.height), 1, labels.Count, 24, 10, 10, false);
            labelInfo.CreateGuiItemsGroup(labels.ToArray(), labelRects, GuiItemType.TEXTFIELD, new GuiItemColor(normal: GuiColor.White));
            buttonRects = SNWindow.SetGridItemsRect(new Rect(drawrect.x + drawrect.width / 2, drawrect.y, drawrect.width / 2, drawrect.height), 1, buttons.Count, 24, 10, 10, false);
            buttonInfo.CreateGuiItemsGroup(buttons.ToArray(), buttonRects, GuiItemType.NORMALBUTTON, new GuiItemColor(normal: GuiColor.White));
        }

        public void OnGUI()
        {
            SNWindow.CreateWindow(windowRect, "QuickSlot Extender Configuration Window", false, false);
                        
            GUI.FocusControl("QuickSlotExtender.ConfigUI");

            labelInfo.DrawGuiItemsGroup();

            int sBtn = buttonInfo.DrawGuiItemsGroup();

            if (sBtn != -1)
            {
                StartAssignment(buttons[sBtn]);
                selected = sBtn;
                buttonInfo[sBtn].Name = "Press any key!";
            }

            float lastY = SNWindow.GetNextYPos(ref buttonRects);

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

        private void StartAssignment(object keyName)
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

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].Equals(newKey))
                {
                    if (keyCount == 0)
                        isFirst = i;

                    keyCount++;
                }
            }

            if (keyCount > 0 && isFirst != selected)
            {
                SNLogger.Log($"[{Config.PROGRAM_NAME}] Error! Duplicate keybind found, swapping keys...");
                buttons[isFirst] = buttons[selected];
                buttonInfo[isFirst].Name = buttons[selected];
            }

            buttons[selected] = newKey;
            buttonInfo[selected].Name = buttons[selected];
            selected = -1;            

            yield return null;
        }

        private IEnumerator WaitForKey()
        {
            while (!keyEvent.isKey)
            {
                yield return null;
            }
        }

        private void SaveAndExit()
        {            
            for (int i = 0; i < labels.Count; i++)
            {
                Config.Section_hotkeys[labels[i]] = buttons[i];
            }

            Config.WriteConfig();
            Config.SetKeyBindings();
            Main.GameInput_OnBindingsChanged();
            Destroy(Instance);
        }

        public ConfigUI()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(ConfigUI)) as ConfigUI;

                if (Instance == null)
                {
                    GameObject go = new GameObject().AddComponent<ConfigUI>().gameObject;
                    go.name = "QuickSlotExtender.ConfigUI";
                    Instance = go.GetComponent<ConfigUI>();
                }
            }            
        }
    }    
}
