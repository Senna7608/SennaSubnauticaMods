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

        public class ButtonInfo
        {
            public string Name { get; set; }
            public bool Enabled { get; set; }
            public bool Pressed { get; set; }
            public BUTTONTYPE Type { get; set; }
            public bool Bold { get; set; }
        }        

        public static Rect CreatePopupWindow(Rect windowRect, object title, bool isTimeLeft = false, bool darkerBg = false)
        {
            int titleHeight = Screen.height / 45;

            GUI.Box(windowRect, "");
            
            if (darkerBg)
                GUI.Box(windowRect, "");

            if (title != null)
            {
                GUI.Box(new Rect(windowRect.x, windowRect.y, windowRect.width, titleHeight), "");                

                if (isTimeLeft)
                {
                    GUI.Label(new Rect(windowRect.x + 5, windowRect.y, windowRect.width * 0.85f, titleHeight), title.ToString());
                    GUI.Label(new Rect(windowRect.x + windowRect.width * 0.85f, windowRect.y, windowRect.width, titleHeight), DateTime.Now.ToString("HH:mm:ss"));
                }
                else
                    GUI.Label(new Rect(windowRect.x + 5, windowRect.y, windowRect.width, titleHeight), title.ToString());

                return new Rect(windowRect.x, windowRect.y + titleHeight, windowRect.width, windowRect.height - titleHeight);
            }

            return windowRect;
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

        public static void CreateItemsGrid(Rect rect, float space, int columns, List<string>items, GUI_ITEM itemType = GUI_ITEM.LABEL)
        {            
            float labelWidth = (rect.width - ((columns + 1) * space)) / columns;
            int rows = CeilToInt(items.Count / (float)columns);
            int itemHeight = Screen.height / 45;

            int row = 0;
            int column = 0;

            for (int i = 0; i < items.Count; i++)
            {                
                if (row == rows)
                {                   
                    row = 0;
                    column++;
                }
                
                switch (itemType)
                {
                    case GUI_ITEM.LABEL:
                        GUI.Label(new Rect(rect.x + space + (column * (labelWidth + space)), rect.y + space + (row * (itemHeight + space)), labelWidth, itemHeight), items[i]);
                        break;

                    case GUI_ITEM.TEXTFIELD:
                        GUI.TextField(new Rect(rect.x + space + (column * (labelWidth + space)), rect.y + space + (row * (itemHeight + space)), labelWidth, itemHeight), items[i]);
                        break;                    
                }
                
                row++;                               
            }            
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

                if (GUI.Button(new Rect(rect.x + space + (column * (calcWidth + space)), rect.y + space + (row * (buttonHeight + space)), calcWidth, buttonHeight), Buttons[i].Name, GetGUIStyle(Buttons[i])))
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
        
        public static GUIStyle Normal;        
        public static GUIStyle Toggle;
        public static GUIStyle Tab;
        public static GUIStyle Label;
        public static GUIStyle Box;

        public static bool InitGUIStyles()
        {            
            Normal = new GUIStyle(GUI.skin.button)
            {                
                alignment = TextAnchor.MiddleCenter
            };           

            Toggle = new GUIStyle(GUI.skin.button)
            {                
                alignment = TextAnchor.MiddleCenter
            };

            Tab = new GUIStyle(GUI.skin.button)
            {                
                alignment = TextAnchor.MiddleCenter
            };

            Label = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft                
            };

            Box = new GUIStyle(GUI.skin.box)
            {                
                alignment = TextAnchor.MiddleCenter
            };            
            return true;
        }

        public static GUIStyle GetGUIStyle(ButtonInfo buttonInfo = null, BUTTONTYPE buttonType = BUTTONTYPE.NORMAL_CENTER)
        {                                 
            if (buttonInfo == null)
            {
                buttonInfo = new ButtonInfo()
                {
                    Name = "",
                    Bold = false,
                    Enabled = true,
                    Pressed = false,
                    Type = buttonType
                };                
            }


            switch(buttonInfo.Type)
            {
                case BUTTONTYPE.NORMAL_CENTER:
                    if (buttonInfo.Pressed)
                    {
                        Normal.normal.textColor = Color.green;
                        Normal.hover.textColor = Color.yellow;                        
                    }
                    else
                    {
                        Normal.normal.textColor = Color.gray;
                        Normal.hover.textColor = Color.white;                        
                    }

                    if (buttonInfo.Bold)
                        Normal.fontStyle = FontStyle.Bold;
                    else
                        Normal.fontStyle = FontStyle.Normal;

                    Normal.active.textColor = Color.green;
                    Normal.alignment = TextAnchor.MiddleCenter;
                    Normal.fontSize = Screen.height / 80;
                    
                    return Normal;

                case BUTTONTYPE.NORMAL_LEFTALIGN:
                    if (buttonInfo.Pressed)
                    {
                        Normal.normal.textColor = Color.green;
                        Normal.hover.textColor = Color.yellow;                        
                    }
                    else
                    {
                        Normal.normal.textColor = Color.gray;
                        Normal.hover.textColor = Color.white;                        
                    }

                    if (buttonInfo.Bold)
                        Normal.fontStyle = FontStyle.Bold;
                    else
                        Normal.fontStyle = FontStyle.Normal;

                    Normal.active.textColor = Color.green;
                    Normal.alignment = TextAnchor.MiddleLeft;
                    Normal.fontSize = Screen.height / 80;
                    return Normal;

                case BUTTONTYPE.TOGGLE_CENTER:
                    if (buttonInfo.Pressed)
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

                    if (buttonInfo.Bold)
                        Toggle.fontStyle = FontStyle.Bold;
                    else
                        Toggle.fontStyle = FontStyle.Normal;

                    Toggle.fontSize = Screen.height / 80;
                    return Toggle;

                case BUTTONTYPE.TAB_CENTER:
                    if (buttonInfo.Pressed)
                    {
                        Tab.normal.textColor = Color.green;
                        Tab.hover.textColor = Color.green;
                    }
                    else
                    {
                        Tab.normal.textColor = Color.gray;
                        Tab.hover.textColor = Color.white;                        
                    }

                    if (buttonInfo.Bold)
                        Tab.fontStyle = FontStyle.Bold;
                    else
                        Tab.fontStyle = FontStyle.Normal;

                    Tab.active.textColor = Color.green;

                    Tab.fontSize = Screen.height / 80;
                    return Tab;

                default:
                    Normal.normal.textColor = Color.gray;
                    Normal.hover.textColor = Color.white;
                    Normal.active.textColor = Color.green;
                    Normal.fontStyle = FontStyle.Normal;                    
                    Normal.alignment = TextAnchor.MiddleLeft;

                    Normal.fontSize = Screen.height / 80;
                    return Normal;
            }
        }
    }
}
