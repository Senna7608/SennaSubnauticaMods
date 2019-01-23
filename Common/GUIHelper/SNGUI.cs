using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.GUIHelper
{
    public static class SNGUI
    {
        public enum GuiItemType
        {
            NORMALBUTTON,
            TOGGLEBUTTON,
            TAB,
            LABEL,
            TEXTFIELD,
            TEXTAREA,
            BOX,
            DROPDOWN
        }

        public interface IGuiItem
        {
            bool Enabled { get; set; }
            FontStyle FontStyle { get; set; }
            Color HoverColor { get; set; }
            string Name { get; set; }
            Color NormalColor { get; set; }
            Rect Rect { get; set; }
            TextAnchor TextAnchor { get; set; }
            GuiItemType Type { get; set; }
        }

        public class GuiItem : IGuiItem
        {
            public GuiItem()
            {
            }

            public GuiItem(string name, GuiItemType type, bool enabled, Rect rect, Color normalColor, Color hoverColor, FontStyle fontStyle, TextAnchor textAnchor)
            {
                Name = name;
                Type = type;
                Enabled = enabled;
                Rect = rect;
                NormalColor = normalColor;
                HoverColor = hoverColor;
                FontStyle = fontStyle;
                TextAnchor = textAnchor;
            }

            public string Name { get; set; }
            public GuiItemType Type { get; set; }
            public bool Enabled { get; set; }
            public Rect Rect { get; set; }
            public Color NormalColor { get; set; }
            public Color HoverColor { get; set; }
            public FontStyle FontStyle { get; set; }
            public TextAnchor TextAnchor { get; set; }
        }

        public static bool CreateGuiItemsGroup(string[] names, Rect[] rects, GuiItemType type, ref List<GuiItem> guiItems, Color normalColor, Color hoverColor,
                                               bool enabled = true, FontStyle fontStyle = FontStyle.Normal, TextAnchor textAnchor = TextAnchor.MiddleCenter)
        {
            guiItems.Clear();

            for (int i = 0; i < names.Length; i++)
            {
                guiItems.Add(new GuiItem()
                {
                    Name = names[i],
                    Type = type,
                    Enabled = enabled,
                    Rect = rects[i],
                    NormalColor = normalColor,
                    HoverColor = hoverColor,
                    FontStyle = fontStyle,
                    TextAnchor = textAnchor
                });
            }

            return true;
        }

        public static bool AddGuiItemToGroup(string name, Rect rect, GuiItemType type, ref List<GuiItem> guiItems, Color normalColor, Color hoverColor,
                                               bool enabled = true, FontStyle fontStyle = FontStyle.Normal, TextAnchor textAnchor = TextAnchor.MiddleCenter)
        {
            
            guiItems.Add(new GuiItem()
            {
                Name = name,
                Type = type,
                Enabled = enabled,
                Rect = rect,
                NormalColor = normalColor,
                HoverColor = hoverColor,
                FontStyle = fontStyle,
                TextAnchor = textAnchor
            });
           
            return true;
        }

        public static void SetGuiItemsGroupLabel(string name, Rect rect, ref List<GuiItem> guiItems, Color normalColor, Color hoverColor,
                                                 FontStyle fontStyle = FontStyle.Normal, TextAnchor textAnchor = TextAnchor.MiddleLeft)
        {
            guiItems.Add(new GuiItem()
            {
                Name = name,
                Type = GuiItemType.LABEL,
                Enabled = true,
                Rect = rect,
                NormalColor = normalColor,
                HoverColor = hoverColor,
                FontStyle = fontStyle,
                TextAnchor = textAnchor
            });
        }

        //to be called from OnGui
        public static int DrawGuiItemsGroup(ref List<GuiItem> guiItems)
        {
            for (int i = 0; i < guiItems.Count; i++)
            {
                if (!guiItems[i].Enabled)
                {
                    continue;
                }

                switch (guiItems[i].Type)
                {
                    case GuiItemType.NORMALBUTTON:
                        if (GUI.Button(guiItems[i].Rect, guiItems[i].Name, SNStyles.GetGuiStyle(guiItems[i])))
                        {
                            return i;
                        }
                        break;

                    case GuiItemType.LABEL:
                        GUI.Label(guiItems[i].Rect, guiItems[i].Name, SNStyles.GetGuiStyle(guiItems[i]));
                        break;

                    case GuiItemType.TEXTFIELD:
                        GUI.TextField(guiItems[i].Rect, guiItems[i].Name, SNStyles.GetGuiStyle(guiItems[i]));
                        break;
                }
            }

            return -1;
        }
    }
}
