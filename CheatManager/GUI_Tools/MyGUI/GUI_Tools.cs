using UnityEngine;

namespace Common.MyGUI
{
    public static class GUI_Tools
    {
        public enum GUISTYLE
        {
            Label = 0,
            ButtonNormal = 1,
            ButtonNormalCenter = 2,
            ButtonPressed = 3,
            ToggleButtonNormal = 4,
            ToggleButtonPressed = 5,
            SelectionGrid = 6,
            ScrollView = 7,
            Box = 8
        }

        public static Rect CreatePopupWindow(Rect windowRect, string title)
        {
            GUI.Box(windowRect, "");
            GUI.Box(new Rect(windowRect.x, windowRect.y, windowRect.width, 23), title);
            return new Rect(windowRect.x, windowRect.y + 23, windowRect.width, windowRect.height - 23);
        }


        public static int CreateNormalButtonsGrid(Rect targetRect, int columns, string[] buttonText, GUIStyle style)
        {           
            float x = targetRect.x;
            float y = targetRect.y + 5;
            float buttonWidth = (targetRect.width - ((columns + 1) * 5)) / columns;
            int row = 1;
            int column = 1;

            for (int i = 0; i < buttonText.Length; i++)
            {
                if (i == (row * columns))
                {
                    y += 27;

                    if (y > (targetRect.y + targetRect.height))
                    {
                        return 255;
                    }

                    row++;
                    column = 1;
                    x = targetRect.x;
                }
                
                if (GUI.Button(new Rect(x + 5, y, buttonWidth, 22), buttonText[i], style))
                {
                    return i + 1;
                }

                x += buttonWidth + 5;
                column++;
            }
            return 0;
        }


        public static GUIStyle SetStyle(GUISTYLE style)
        {
            GUIStyle gUIStyle;

            switch (style)
            {
                case GUISTYLE.Label:
                    gUIStyle = new GUIStyle(GUI.skin.label);
                    gUIStyle.normal.textColor = Color.white;
                    gUIStyle.fontStyle = FontStyle.Bold;
                    gUIStyle.alignment = TextAnchor.MiddleLeft;
                    return gUIStyle;

                case GUISTYLE.ButtonNormal:
                    gUIStyle = new GUIStyle(GUI.skin.button);
                    gUIStyle.normal.textColor = Color.gray;
                    gUIStyle.hover.textColor = Color.white;
                    gUIStyle.active.textColor = Color.green;
                    gUIStyle.fontStyle = FontStyle.Bold;
                    return gUIStyle;

                case GUISTYLE.ButtonNormalCenter:
                    gUIStyle = new GUIStyle(GUI.skin.button);
                    gUIStyle.normal.textColor = Color.gray;
                    gUIStyle.hover.textColor = Color.white;
                    gUIStyle.onNormal.textColor = Color.green;
                    gUIStyle.onHover.textColor = Color.green;
                    gUIStyle.active.textColor = Color.green;
                    gUIStyle.onActive.textColor = Color.green;
                    gUIStyle.fontStyle = FontStyle.Bold;
                    return gUIStyle;

                case GUISTYLE.ToggleButtonNormal:
                    gUIStyle = new GUIStyle(GUI.skin.button);
                    gUIStyle.normal.textColor = Color.red;
                    gUIStyle.hover.textColor = Color.red;
                    gUIStyle.active.textColor = Color.green;
                    gUIStyle.fontStyle = FontStyle.Bold;
                    return gUIStyle;

                case GUISTYLE.ToggleButtonPressed:
                    gUIStyle = new GUIStyle(GUI.skin.button);
                    gUIStyle.normal.textColor = Color.green;
                    gUIStyle.hover.textColor = Color.green;
                    gUIStyle.active.textColor = Color.red;
                    gUIStyle.fontStyle = FontStyle.Bold;
                    return gUIStyle;

                case GUISTYLE.SelectionGrid:
                    gUIStyle = new GUIStyle(GUI.skin.button);
                    gUIStyle.normal.textColor = Color.gray;
                    gUIStyle.hover.textColor = Color.white;
                    gUIStyle.onNormal.textColor = Color.green;
                    gUIStyle.onHover.textColor = Color.green;
                    gUIStyle.active.textColor = Color.green;
                    gUIStyle.onActive.textColor = Color.green;
                    gUIStyle.fontStyle = FontStyle.Bold;
                    gUIStyle.alignment = TextAnchor.MiddleLeft;
                    return gUIStyle;

                case GUISTYLE.ScrollView:
                    gUIStyle = new GUIStyle(GUI.skin.button);
                    gUIStyle.normal.textColor = Color.gray;
                    gUIStyle.hover.textColor = Color.white;
                    gUIStyle.onNormal.textColor = Color.green;
                    gUIStyle.onHover.textColor = Color.green;
                    gUIStyle.active.textColor = Color.green;
                    gUIStyle.onActive.textColor = Color.green;
                    gUIStyle.fontStyle = FontStyle.Bold;
                    gUIStyle.alignment = TextAnchor.MiddleLeft;
                    return gUIStyle;

                case GUISTYLE.Box:
                    gUIStyle = new GUIStyle(GUI.skin.box);
                    gUIStyle.normal.textColor = Color.white;
                    gUIStyle.fontStyle = FontStyle.Bold;
                    return gUIStyle;

                default:
                    return null;
            }
        }




    }
}
