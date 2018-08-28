using System.Collections.Generic;
using UnityEngine;

namespace GUIHelper
{
    internal class Tools
    {
        internal enum BUTTONTYPE
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

        internal class ButtonInfo
        {
            public string Name { get; set; }
            public bool Enabled { get; set; }
            public bool Pressed { get; set; }
            public BUTTONTYPE Type { get; set; }            
        }

        internal static Rect CreatePopupWindow(Rect windowRect, object title)
        {

            GUI.Box(windowRect, "");

            if (title != null)
            {
                GUI.Box(new Rect(windowRect.x, windowRect.y, windowRect.width, 23), title.ToString());
                return new Rect(windowRect.x, windowRect.y + 23, windowRect.width, windowRect.height - 23);
            }

            return windowRect;
        }

        internal static bool CreateButtonsList(string[] names, BUTTONTYPE type, ref List<ButtonInfo> buttonInfos)
        {
            for (int i = 0; i < names.Length; i++)
            {
                buttonInfos.Add(new ButtonInfo()
                {
                    Name = names[i],
                    Enabled = true,
                    Type = type,
                    Pressed = false
                });
            }

            return true;
        }

        internal static int DivideRoundUP(float a, float b)
        {
            int x = (int)(a / b);

            if (x * b < a)
                return x + 1;

            return x;
        }


        internal static int CreateButtonsGrid(Rect targetRect, float space, int columns, List<ButtonInfo> Buttons, out float lastYcoord)
        {
            float x = targetRect.x;
            float y = targetRect.y + space;
            float buttonWidth = (targetRect.width - ((columns + 1) * 5)) / columns;
            int totalRows = DivideRoundUP(Buttons.Count, columns);

            int row = 1;
            int column = 1;

            for (int i = 0; i < Buttons.Count; i++)
            {
                if (!Buttons[i].Enabled)
                {
                    continue;
                }

                if (i == (row * columns))
                {
                    y += 22 + space;

                    if (y > targetRect.height)
                    {
                        lastYcoord = y;
                        return -1;
                    }

                    row++;
                    column = 1;
                    x = targetRect.x;
                }

                if (GUI.Button(new Rect(x + 5, y, buttonWidth, 22), Buttons[i].Name, GetCustomStyle(Buttons[i].Pressed, Buttons[i].Type)))
                {
                    lastYcoord = targetRect.y + (totalRows * (22 + space));
                    return i;
                }

                x += buttonWidth + 5;
                column++;
            }

            lastYcoord = targetRect.y + (totalRows * (22 + space));
            return -1;
        }

        internal static GUIStyle Normal;
        internal static GUIStyle Toggle;
        internal static GUIStyle Tab;
        internal static GUIStyle Label;        
        internal static GUIStyle Box;

        internal static bool SetCustomStyles()
        {
            Normal = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };            

            Toggle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

            Tab = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

            Label = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
            };

            Box = new GUIStyle(GUI.skin.box)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
            };            
            return true;
        }

        internal static GUIStyle GetCustomStyle(bool pressed, BUTTONTYPE type)
        {
            switch(type)
            {
                case BUTTONTYPE.NORMAL_CENTER:
                    if (pressed)
                    {
                        Normal.normal.textColor = Color.green;
                        Normal.hover.textColor = Color.yellow;
                        Normal.active.textColor = Color.green;
                        Normal.alignment = TextAnchor.MiddleCenter;
                    }
                    else
                    {
                        Normal.normal.textColor = Color.gray;
                        Normal.hover.textColor = Color.white;
                        Normal.active.textColor = Color.green;
                        Normal.alignment = TextAnchor.MiddleCenter;
                    }                    
                    return Normal;

                case BUTTONTYPE.NORMAL_LEFTALIGN:
                    if (pressed)
                    {
                        Normal.normal.textColor = Color.green;
                        Normal.hover.textColor = Color.yellow;
                        Normal.active.textColor = Color.green;
                        Normal.alignment = TextAnchor.MiddleLeft;
                    }
                    else
                    {
                        Normal.normal.textColor = Color.gray;
                        Normal.hover.textColor = Color.white;
                        Normal.active.textColor = Color.green;
                        Normal.alignment = TextAnchor.MiddleLeft;
                    }
                    return Normal;

                case BUTTONTYPE.TOGGLE_CENTER:
                    if (pressed)
                    {
                        Toggle.normal.textColor = Color.green;
                        Toggle.hover.textColor = Color.green;
                        Toggle.active.textColor = Color.red;
                    }
                    else
                    {
                        Toggle.normal.textColor = Color.red;
                        Toggle.hover.textColor = Color.red;
                        Toggle.active.textColor = Color.green;
                    }
                    return Toggle;

                case BUTTONTYPE.TAB_CENTER:
                    if (pressed)
                    {
                        Tab.normal.textColor = Color.green;
                        Tab.hover.textColor = Color.green;
                        Tab.active.textColor = Color.green;
                    }
                    else
                    {
                        Tab.normal.textColor = Color.gray;
                        Tab.hover.textColor = Color.white;
                        Tab.active.textColor = Color.green;
                    }                    
                    return Tab;
                default:
                    return Normal;
            }
        }
    }
}
