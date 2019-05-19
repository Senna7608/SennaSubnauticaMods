using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.GUIHelper;

namespace SlotExtender.Configuration
{
    internal class SEConfigUI : MonoBehaviour
    {
        internal SEConfigUI Instance { get; private set; }
        
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
        private static int dropdownSelection = SEConfig.MAXSLOTS - 5;


        public void Awake()
        {
            useGUILayout = false;
            Instance = this;            
            InitItems();         
        }
       
        private void InitItems()
        {
            foreach (KeyValuePair<string, string> key in SEConfig.Section_hotkeys)
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

            itemInfo.CreateGuiItemsGroup(hotkeyLabels, itemsRect, GuiItemType.LABEL, new GuiItemColor(normal: GuiColor.White), fontStyle: FontStyle.Bold, textAnchor: TextAnchor.MiddleLeft);

            buttonInfo.CreateGuiItemsGroup(hotkeyButtons, buttonsRect, GuiItemType.NORMALBUTTON, new GuiItemColor(), fontStyle: FontStyle.Bold, textAnchor: TextAnchor.MiddleCenter);

            itemInfo.SetGuiItemsGroupLabel("Functions", itemsRect.GetLast(),  new GuiItemColor());

            buttonInfo.SetGuiItemsGroupLabel("Hotkeys", buttonsRect.GetLast(),  new GuiItemColor());
        }

        public void OnGUI()
        {
            SNWindow.CreateWindow(new Rect(0, 0, Screen.width / 6, hotkeyLabels.Count * 47), "SlotExtender Configuration Window", false, false);

            GUI.FocusControl("SlotExtender.ConfigUI");

            itemInfo.DrawGuiItemsGroup();

            int sBtn = buttonInfo.DrawGuiItemsGroup();

            if (sBtn != -1)
            {
                StartAssignment(hotkeyButtons[sBtn]);
                selected = sBtn;
                buttonInfo[sBtn].Name = "Press any key!";
            }                       

            SNDropDown.CreateDropdown(buttonsRect[buttonsRect.Count - 2], ref isVisible, ref dropdownSelection, dropDownContent);

            float y = itemsRect[itemsRect.Count - 2].y + space * 2 + itemsRect[0].height;

            if (GUI.Button(new Rect(itemsRect[0].x, y, itemsRect[0].width, Screen.height / 22.5f), "Save", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON)))
            {
                SaveAndExit();
            }

            if (!isVisible)
            {
                if (GUI.Button(new Rect(buttonsRect[0].x, y, buttonsRect[0].width, Screen.height / 22.5f), "Cancel", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON)))
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
                SNLogger.Log($"[{SEConfig.PROGRAM_NAME}] Warning! Duplicate keybind found, swapping keys...");
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
                SEConfig.Section_hotkeys[hotkeyLabels[i]] = hotkeyButtons[i];
            }

            int.TryParse(dropDownContent[dropdownSelection].text, out int result);

            if (result != SEConfig.MAXSLOTS)
            {
                ErrorMessage.AddMessage("SlotExtender Warning!\nMaxSlots changed!\nPlease restart the game!");
                SEConfig.MAXSLOTS = result;
            }

            SEConfig.WriteConfig();
            SEConfig.SetKeyBindings();
            Main.GameInput_OnBindingsChanged();
            Destroy(Instance);
        }

        public SEConfigUI()
        {
            if (Instance.IsNull())
            {
                Instance = FindObjectOfType(typeof(SEConfigUI)) as SEConfigUI;

                if (Instance.IsNull())
                {
                    GameObject se_configUI = new GameObject("SEConfigUI");
                    Instance = se_configUI.GetOrAddComponent<SEConfigUI>();                    
                }
            }
            else
            {
                Instance.Awake();
            }
        }
    }    
}
