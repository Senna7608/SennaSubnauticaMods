using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.GUIHelper;
using Common.Helpers;

namespace QuickSlotExtender.Configuration
{
    internal class QSEConfigUI : MonoBehaviour
    {
        public QSEConfigUI Instance;

        private Rect windowRect;
        private List<Rect> hotkeyLabelsRect;
        private List<Rect> hotkeyButtonsRect;

        private const int space = 10;
        private int selected = -1;
        private Event keyEvent;
        private string newKey;
        private bool waitingForKey = false;
        private static bool isMaxSlotsDropDownVisible = false;
        private static bool isTextColorDropDownVisible = false;
        private static int MaxSlotDropDownSelection = QSEConfig.MAXSLOTS - 5;
        private static int TextColorDropDownSelection = ColorHelper.GetColorInt(QSEConfig.TEXTCOLOR);

        private List<string> hotkeyLabels = new List<string>();
        private List<string> hotkeyButtons = new List<string>();

        private List<GuiItem> guiItem_Buttons = new List<GuiItem>();
        private List<GuiItem> guiItem_Labels = new List<GuiItem>();

        private static GUIContent[] MaxSlotDropDownContent = new GUIContent[]
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

        private static GUIContent[] TextColorDropDownContent = new GUIContent[]
        {
            new GUIContent("Red"),
            new GUIContent("Green"),
            new GUIContent("Blue"),
            new GUIContent("Yellow"),
            new GUIContent("White"),
            new GUIContent("Magenta"),
            new GUIContent("Cyan"),
            new GUIContent("Orange"),
            new GUIContent("Lime"),
            new GUIContent("Amethyst"),
            new GUIContent("LightBlue")
        };

        public void Awake()
        {
            useGUILayout = false;
            InitItems();
        }

        private void InitItems()
        {
            foreach (KeyValuePair<string, string> key in QSEConfig.Section_hotkeys)
            {
                hotkeyLabels.Add(key.Key);
                hotkeyButtons.Add(key.Value);
            }

            hotkeyLabels.Add("MaxSlots");

            hotkeyLabels.Add("TextColor");

            windowRect = SNWindow.InitWindowRect(new Rect(0, 0, Screen.width / 5, hotkeyLabels.Count * 48));

            hotkeyLabelsRect = SNWindow.SetGridItemsRect(new Rect(windowRect.x, windowRect.y, windowRect.width / 2, windowRect.height),
                                                1, hotkeyLabels.Count, Screen.height / 45, space, space, false, true);

            hotkeyButtonsRect = SNWindow.SetGridItemsRect(new Rect(windowRect.x + windowRect.width / 2, windowRect.y, windowRect.width / 2, windowRect.height),
                                                  1, hotkeyButtons.Count + 2, Screen.height / 45, space, space, false, true);

            guiItem_Labels.CreateGuiItemsGroup(hotkeyLabels, hotkeyLabelsRect, GuiItemType.LABEL, new GuiItemColor(normal: GuiColor.White), fontStyle: FontStyle.Bold, textAnchor: TextAnchor.MiddleLeft);

            guiItem_Buttons.CreateGuiItemsGroup(hotkeyButtons, hotkeyButtonsRect, GuiItemType.NORMALBUTTON, new GuiItemColor(), fontStyle: FontStyle.Bold, textAnchor: TextAnchor.MiddleCenter);

            guiItem_Labels.SetGuiItemsGroupLabel("Functions", hotkeyLabelsRect.GetLast(), new GuiItemColor(normal: GuiColor.Green));

            guiItem_Buttons.SetGuiItemsGroupLabel("Hotkeys", hotkeyButtonsRect.GetLast(), new GuiItemColor(normal: GuiColor.Green));
        }

        public void OnGUI()
        {
            SNWindow.CreateWindow(new Rect(0, 0, Screen.width / 5, hotkeyLabels.Count * 48), $"QuickSlot Extender Configuration Window ({QSEConfig.PROGRAM_VERSION})", false, false);

            GUI.FocusControl("QSEConfigUI");

            guiItem_Labels.DrawGuiItemsGroup();

            GuiItemEvent sBtn = guiItem_Buttons.DrawGuiItemsGroup();

            if (sBtn.ItemID != -1)
            {
                StartAssignment(hotkeyButtons[sBtn.ItemID]);
                selected = sBtn.ItemID;
                guiItem_Buttons[sBtn.ItemID].Name = "Press any key!";
            }

            SNDropDown.CreateDropdown(hotkeyButtonsRect[hotkeyButtonsRect.Count - 3], ref isMaxSlotsDropDownVisible, ref MaxSlotDropDownSelection, MaxSlotDropDownContent);

            if (!isMaxSlotsDropDownVisible)
            {
                SNDropDown.CreateDropdown(hotkeyButtonsRect[hotkeyButtonsRect.Count - 2], ref isTextColorDropDownVisible, ref TextColorDropDownSelection, TextColorDropDownContent);
            }

            float y = hotkeyLabelsRect[hotkeyLabelsRect.Count - 2].y + space * 2 + hotkeyLabelsRect[0].height;

            if (GUI.Button(new Rect(hotkeyLabelsRect[0].x, y, hotkeyLabelsRect[0].width, Screen.height / 25f), "Save", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON)))
            {
                SaveAndExit();
            }

            if (!isMaxSlotsDropDownVisible && !isTextColorDropDownVisible)
            {
                if (GUI.Button(new Rect(hotkeyButtonsRect[0].x, y, hotkeyButtonsRect[0].width, Screen.height / 25f), "Cancel", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON)))
                {
                    Destroy(this);
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
                SNLogger.Warn("QuickSlotExtender", "Duplicate keybind found, swapping keys...");
                hotkeyButtons[isFirst] = hotkeyButtons[selected];
                guiItem_Buttons[isFirst].Name = hotkeyButtons[selected];
            }

            hotkeyButtons[selected] = newKey;
            guiItem_Buttons[selected].Name = hotkeyButtons[selected];
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
                QSEConfig.Section_hotkeys[hotkeyLabels[i]] = hotkeyButtons[i];
            }

            int.TryParse(MaxSlotDropDownContent[MaxSlotDropDownSelection].text, out int MaxSlotsResult);

            if (MaxSlotsResult != QSEConfig.MAXSLOTS)
            {
                QSEConfig.MAXSLOTS = MaxSlotsResult;
            }

            Color TextColorResult = ColorHelper.GetColor(TextColorDropDownContent[TextColorDropDownSelection].text);

            if (TextColorResult != QSEConfig.TEXTCOLOR)
            {
                QSEConfig.TEXTCOLOR = TextColorResult;
            }

            QSEConfig.WriteConfig();
            QSEConfig.SetKeyBindings();
            QuickSlotExtender.GameInput_OnBindingsChanged();
            ErrorMessage.AddMessage("Quick Slot Extender message:\nConfiguration saved.");
            Destroy(this);
        }

        public QSEConfigUI()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(QSEConfigUI)) as QSEConfigUI;

                if (Instance == null)
                {
                    GameObject qsez_configUI = new GameObject("QSEConfigUI");
                    Instance = qsez_configUI.AddComponent<QSEConfigUI>();
                }
            }
            else
            {
                Instance.Awake();
            }
        }
    }
}
