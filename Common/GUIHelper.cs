using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

namespace Common
{
    public class GUIHelper
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

        public enum GUI_ITEM
        {
            LABEL,
            TEXTFIELD           
        }

        public enum GUI_ALIGN
        {
            RIGHTDOWN,
            DOWNRIGHT
        }

        public class ButtonInfo
        {
            public string Name { get; set; }
            public bool Enabled { get; set; }
            public bool Pressed { get; set; }
            public BUTTONTYPE Type { get; set; }            
        }

        public static Rect CreatePopupWindow(Rect windowRect, object title, bool isTimeLeft = false, bool darkerBg = false)
        {
            GUI.Box(windowRect, "");
            
            if (darkerBg)
                GUI.Box(windowRect, "");

            if (title != null)
            {
                GUI.Box(new Rect(windowRect.x, windowRect.y, windowRect.width, 23), title.ToString());

                if (isTimeLeft)
                {
                    GUI.Label(new Rect(windowRect.x + windowRect.width - 60, windowRect.y, windowRect.width, 23), DateTime.Now.ToString("HH:mm:ss"));
                }

                return new Rect(windowRect.x, windowRect.y + 23, windowRect.width, windowRect.height - 23);
            }

            return windowRect;
        }

        public static bool CreateButtonsList(string[] names, BUTTONTYPE type, ref List<ButtonInfo> buttonInfos)
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

        public static void CreateItemsGrid(Rect rect, float space, int columns, List<string>items, GUI_ITEM itemsType = GUI_ITEM.LABEL)
        {            
            float labelWidth = (rect.width - ((columns + 1) * space)) / columns;
            int rows = CeilToInt(items.Count / (float)columns);                     
            
            int row = 0;
            int column = 0;

            for (int i = 0; i < items.Count; i++)
            {                
                if (row == rows)
                {                   
                    row = 0;
                    column++;
                }
                
                switch (itemsType)
                {
                    case GUI_ITEM.LABEL:
                        GUI.Label(new Rect(rect.x + space + (column * (labelWidth + space)), rect.y + space + (row * (22 + space)), labelWidth, 22), items[i]);
                        break;

                    case GUI_ITEM.TEXTFIELD:
                        GUI.TextField(new Rect(rect.x + space + (column * (labelWidth + space)), rect.y + space + (row * (22 + space)), labelWidth, 22), items[i]);
                        break;                    
                }
                
                row++;                               
            }            
        }


        public static int CreateButtonsGrid(Rect rect, float space, int columns, List<ButtonInfo> Buttons, out float lastYcoord, GUI_ALIGN align = GUI_ALIGN.RIGHTDOWN)
        {            
            float calcWidth = (rect.width - ((columns + 1) * space)) / columns;
            int rows = CeilToInt(Buttons.Count / (float)columns);

            int row = 0;
            int column = 0;

            for (int i = 0; i < Buttons.Count; i++)
            {
                if (!Buttons[i].Enabled)
                {
                    continue;
                }                
                
                if (align == GUI_ALIGN.RIGHTDOWN)
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

                if (GUI.Button(new Rect(rect.x + space + (column * (calcWidth + space)), rect.y + space + (row * (22 + space)), calcWidth, 22), Buttons[i].Name, GetCustomStyle(Buttons[i].Pressed, Buttons[i].Type)))
                {
                    lastYcoord = rect.y + (rows * (22 + space));
                    return i;
                }
                
                if (align == GUI_ALIGN.RIGHTDOWN)
                    column++;
                else
                    row++;
            }

            lastYcoord = rect.y + (rows * (22 + space));
            return -1;
        }        

        public static GUIStyle Normal;        
        public static GUIStyle Toggle;
        public static GUIStyle Tab;
        public static GUIStyle Label;
        public static GUIStyle Box;

        public static bool SetCustomStyles()
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

        public static GUIStyle GetCustomStyle(bool pressed, BUTTONTYPE type)
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
