using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

namespace Common.GUIHelper
{
    public static class Button
    {
        public enum BUTTONTYPE
        {
            NORMAL_LEFTALIGN,
            NORMAL_CENTER,
            NORMAL_RIGHTALIGN,

            TOGGLE_LEFTALIGN,
            TOGGLE_CENTER,
            TOGGLE_RIGHTALIGN,

            TAB_LEFTALIGN,
            TAB_CENTER,
            TAB_RIGHTALIGN,
        }

        public class ButtonInfo
        {            
            public string Name { get; set; }
            public bool Enabled { get; set; }
            public bool Pressed { get; set; }
            public BUTTONTYPE Type { get; set; }
            public bool Bold { get; set; }

            public ButtonInfo() { }

            public ButtonInfo(string name, bool enabled, bool pressed, BUTTONTYPE type, bool bold)
            {
                Name = name;
                Enabled = enabled;
                Pressed = pressed;
                Type = type;
                Bold = bold;
            }
        }

        public static bool CreateButtonsGroup(string[] names, BUTTONTYPE type, ref List<ButtonInfo> buttonInfos, bool enabled = true, bool pressed = false, bool bold = false)
        {
            buttonInfos.Clear();

            for (int i = 0; i < names.Length; i++)
            {
                buttonInfos.Add(new ButtonInfo()
                {
                    Name = names[i],
                    Enabled = enabled,
                    Type = type,
                    Pressed = pressed,
                    Bold = bold
                });
            }

            return true;
        }

        public static int CreateButtonsGrid(Rect rect, float space, int columns, List<ButtonInfo> Buttons, out float lastYcoord, bool alignRightDown = true)
        {
            float calcWidth = (rect.width - ((columns + 1) * space)) / columns;
            int rows = CeilToInt(Buttons.Count / (float)columns);
            int buttonHeight = Screen.height / 45;

            int row = 0;
            int column = 0;

            for (int i = 0; i < Buttons.Count; i++)
            {
                if (!Buttons[i].Enabled)
                {
                    continue;
                }

                if (alignRightDown)
                {
                    if (column == columns)
                    {
                        column = 0;
                        row++;
                    }
                }
                else
                {
                    if (row == rows)
                    {
                        row = 0;
                        column++;
                    }
                }

                if (GUI.Button(new Rect(rect.x + space + (column * (calcWidth + space)), rect.y + space + (row * (buttonHeight + space)), calcWidth, buttonHeight), Buttons[i].Name, SNStyles.GetGUIStyle(Buttons[i])))
                {
                    lastYcoord = rect.y + (rows * (buttonHeight + space));
                    return i;
                }

                if (alignRightDown)
                    column++;
                else
                    row++;
            }

            lastYcoord = rect.y + (rows * (buttonHeight + space));
            return -1;
        }

        public static int CreateButtonsGrid(ref Rect[] rects, ref List<ButtonInfo> Buttons)
        {
            if (rects.Length < Buttons.Count)
            {
                throw new ArgumentOutOfRangeException($"Array too small! Increase [{rects.ToString()}] array length!");
            }            

            for (int i = 0; i < Buttons.Count; i++)
            {
                if (!Buttons[i].Enabled)
                {
                    continue;
                }

                if (GUI.Button(rects[i], Buttons[i].Name, SNStyles.GetGUIStyle(Buttons[i])))
                {                
                    return i;
                }
            }

            return -1;
        }
    }
}
