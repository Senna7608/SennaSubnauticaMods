using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Common;
using Common.GUIHelper;

namespace SlotExtender.Configuration
{
    internal class ConfigUI : MonoBehaviour
    {
        internal ConfigUI Instance { get; private set; }
        
        private Rect windowRect;
        private List<Rect> buttonsRect;
        private List<Rect> itemsRect;
        private const int space = 10;       
        private int selected = -1;
        private Event keyEvent;
        private string newKey;
        private bool waitingForKey = false;
        private List<string> hotkeyLabels = new List<string>();
        private List<string> hotkeyButtons = new List<string>();
        private List<GuiItem> buttonInfo = new List<GuiItem>();
        private List<GuiItem> itemInfo = new List<GuiItem>();

        private static GUIContent[] dropDownContent = new GUIContent[]
        {            
            new GUIContent("5"),
            new GUIContent("6"),
            new GUIContent("7"),
            new GUIContent("8"),
            new GUIContent("9"),
            new GUIContent("10"),
            new GUIContent("11"),
            new GUIContent("12")
        };

        private static bool isVisible = false;
        private static int dropdownSelection = Config.MAXSLOTS - 5;


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

            hotkeyLabels.Add("MaxSlots");

            windowRect = SNWindow.InitWindowRect(new Rect(0, 0, Screen.width / 6, hotkeyLabels.Count * 47));

            itemsRect = SNWindow.SetGridItemsRect(new Rect(windowRect.x, windowRect.y, windowRect.width / 2, windowRect.height),
                                                1, hotkeyLabels.Count, Screen.height / 45, space, space, false, true);           

            buttonsRect = SNWindow.SetGridItemsRect(new Rect(windowRect.x + windowRect.width / 2, windowRect.y, windowRect.width / 2, windowRect.height),
                                                  1, hotkeyButtons.Count + 1, Screen.height / 45, space, space, false, true);

            SNGUI.CreateGuiItemsGroup(hotkeyLabels.ToArray(), itemsRect, GuiItemType.LABEL,
                                      ref itemInfo, new GuiItemColor(), fontStyle: FontStyle.Bold, textAnchor: TextAnchor.MiddleLeft);            

            SNGUI.CreateGuiItemsGroup(hotkeyButtons.ToArray(), buttonsRect, GuiItemType.NORMALBUTTON,
                                      ref buttonInfo, new GuiItemColor(), fontStyle: FontStyle.Bold, textAnchor: TextAnchor.MiddleCenter);            

            SNGUI.SetGuiItemsGroupLabel("Functions", itemsRect.GetLast(), ref itemInfo, new GuiItemColor());

            SNGUI.SetGuiItemsGroupLabel("Hotkeys", buttonsRect.GetLast(), ref buttonInfo, new GuiItemColor());
        }

        public void OnGUI()
        {
            SNWindow.CreateWindow(new Rect(0, 0, Screen.width / 6, hotkeyLabels.Count * 47), "SlotExtender Configuration Window", false, false);

            GUI.FocusControl("SlotExtender.ConfigUI");

            SNGUI.DrawGuiItemsGroup(ref itemInfo);

            int sBtn = SNGUI.DrawGuiItemsGroup(ref buttonInfo);

            if (sBtn != -1)
            {
                StartAssignment(hotkeyButtons[sBtn]);
                selected = sBtn;
                buttonInfo[sBtn].Name = "Press any key!";
            }                       

            SNDropDown.CreateDropdown(buttonsRect[buttonsRect.Count - 2], ref isVisible, ref dropdownSelection, dropDownContent);

            float y = itemsRect[itemsRect.Count - 2].y + space * 2 + itemsRect[0].height;

            if (GUI.Button(new Rect(itemsRect[0].x, y, itemsRect[0].width, Screen.height / 22.5f), "Save", SNStyles.GetGuiStyle(GuiItemType.NORMALBUTTON)))
            {
                SaveAndExit();
            }

            if (!isVisible)
            {
                if (GUI.Button(new Rect(buttonsRect[0].x, y, buttonsRect[0].width, Screen.height / 22.5f), "Cancel", SNStyles.GetGuiStyle(GuiItemType.NORMALBUTTON)))
                {
                    Destroy(Instance);
                }
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
                Logger.Log("Error! Duplicate keybind found, swapping keys...");
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
            {
                yield return null;
            }
        }

        private void SaveAndExit()
        {            
            for (int i = 0; i < hotkeyButtons.Count; i++)
            {
                Config.Section_hotkeys[hotkeyLabels[i]] = hotkeyButtons[i];
            }

            int.TryParse(dropDownContent[dropdownSelection].text, out int result);

            if (result != Config.MAXSLOTS)
            {
                ErrorMessage.AddMessage("SlotExtender Warning!\nMaxSlots changed!\nPlease restart the game!");
                Config.MAXSLOTS = result;
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
                    go.name = "SlotExtender.ConfigUI";
                    Instance = go.GetComponent<ConfigUI>();
                }
            }
            else
            {
                Instance.Awake();
            }
        }
    }    
}
