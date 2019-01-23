using System;
using UnityEngine;

namespace Common.GUIHelper
{
    public static class SNStyles
    {
        private static GUIStyle NormalButton;
        private static GUIStyle ToggleButton;
        private static GUIStyle Tab;
        private static GUIStyle Label;
        private static GUIStyle Textfield;
        private static GUIStyle Textarea;
        private static GUIStyle Box;
        private static GUIStyle Dropdown;

        public static bool InitGUIStyles()
        {
            NormalButton = new GUIStyle(GUI.skin.button);
            ToggleButton = new GUIStyle(GUI.skin.button);
            Tab = new GUIStyle(GUI.skin.button);
            Label = new GUIStyle(GUI.skin.label);
            Textfield = new GUIStyle(GUI.skin.textField);
            Textarea = new GUIStyle(GUI.skin.textArea);
            Box = new GUIStyle(GUI.skin.box);
            Dropdown = new GUIStyle(GUI.skin.box);
            return true;
        }

        public static GUIStyle GetGuiStyle(SNGUI.GuiItem guiItem)
        {
            switch(guiItem.Type)
            {
                case SNGUI.GuiItemType.NORMALBUTTON:
                    NormalButton.fontStyle = guiItem.FontStyle;
                    NormalButton.alignment = guiItem.TextAnchor;
                    NormalButton.normal.textColor = guiItem.NormalColor;
                    NormalButton.onNormal.textColor = guiItem.NormalColor;
                    NormalButton.hover.textColor = guiItem.HoverColor;
                    NormalButton.onHover.textColor = guiItem.HoverColor;
                    return NormalButton;

                case SNGUI.GuiItemType.TEXTFIELD:
                    Textfield.fontStyle = guiItem.FontStyle;
                    Textfield.alignment = guiItem.TextAnchor;
                    Textfield.normal.textColor = guiItem.NormalColor;
                    Textfield.onNormal.textColor = guiItem.NormalColor;
                    Textfield.hover.textColor = guiItem.HoverColor;
                    Textfield.onHover.textColor = guiItem.HoverColor;                    
                    return Textfield;
                    
                case SNGUI.GuiItemType.LABEL:
                    Label.fontStyle = guiItem.FontStyle;
                    Label.alignment = guiItem.TextAnchor;
                    Label.normal.textColor = guiItem.NormalColor;
                    Label.onNormal.textColor = guiItem.NormalColor;
                    Label.hover.textColor = guiItem.HoverColor;
                    Label.onHover.textColor = guiItem.HoverColor;
                    return Label;                    
            }

            throw new Exception("Unknown error!");
        }




        public static GUIStyle GetGuiStyle(SNGUI.GuiItemType type)
        {
            switch (type)
            {
                case SNGUI.GuiItemType.NORMALBUTTON:
                    return NormalButton;

                case SNGUI.GuiItemType.TOGGLEBUTTON:
                    return ToggleButton;

                case SNGUI.GuiItemType.TAB:
                    return Tab;

                case SNGUI.GuiItemType.LABEL:
                    return Label;

                case SNGUI.GuiItemType.TEXTFIELD:
                    return Textfield;

                case SNGUI.GuiItemType.TEXTAREA:
                    return Textarea;

                case SNGUI.GuiItemType.BOX:
                    return Box;

                case SNGUI.GuiItemType.DROPDOWN:
                    Dropdown.normal.background = MakeTex(10, 10, new Color(0f, 1f, 0f, 1f));                    
                    return Dropdown;
            }

            throw new Exception("Unknown error!");
        }

        private static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }


        public static GUIStyle GetGUIStyle(Button.ButtonInfo buttonInfo)
        {
            switch (buttonInfo.Type)
            {
                case Button.BUTTONTYPE.NORMAL_CENTER:
                    if (buttonInfo.Pressed)
                    {
                        NormalButton.normal.textColor = Color.green;
                        NormalButton.hover.textColor = Color.yellow;
                    }
                    else
                    {
                        NormalButton.normal.textColor = Color.gray;
                        NormalButton.hover.textColor = Color.white;
                    }

                    if (buttonInfo.Bold)
                        NormalButton.fontStyle = FontStyle.Bold;
                    else
                        NormalButton.fontStyle = FontStyle.Normal;

                    NormalButton.active.textColor = Color.green;
                    NormalButton.alignment = TextAnchor.MiddleCenter;
                    NormalButton.fontSize = Screen.height / 80;

                    return NormalButton;

                case Button.BUTTONTYPE.NORMAL_LEFTALIGN:
                    if (buttonInfo.Pressed)
                    {
                        NormalButton.normal.textColor = Color.green;
                        NormalButton.hover.textColor = Color.yellow;
                    }
                    else
                    {
                        NormalButton.normal.textColor = Color.gray;
                        NormalButton.hover.textColor = Color.white;
                    }

                    if (buttonInfo.Bold)
                        NormalButton.fontStyle = FontStyle.Bold;
                    else
                        NormalButton.fontStyle = FontStyle.Normal;

                    NormalButton.active.textColor = Color.green;
                    NormalButton.alignment = TextAnchor.MiddleLeft;
                    NormalButton.fontSize = Screen.height / 80;
                    return NormalButton;

                case Button.BUTTONTYPE.TOGGLE_CENTER:
                    if (buttonInfo.Pressed)
                    {
                        ToggleButton.normal.textColor = Color.green;
                        ToggleButton.hover.textColor = Color.green;
                        ToggleButton.active.textColor = Color.red;
                    }
                    else
                    {
                        ToggleButton.normal.textColor = Color.red;
                        ToggleButton.hover.textColor = Color.red;
                        ToggleButton.active.textColor = Color.green;
                    }

                    if (buttonInfo.Bold)
                        ToggleButton.fontStyle = FontStyle.Bold;
                    else
                        ToggleButton.fontStyle = FontStyle.Normal;

                    ToggleButton.fontSize = Screen.height / 80;
                    return ToggleButton;

                case Button.BUTTONTYPE.TAB_CENTER:
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
            }

            throw new Exception("Unknown error!");
        }
    }
}
