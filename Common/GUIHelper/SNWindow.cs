using System;
using UnityEngine;

namespace Common.GUIHelper
{
    public static class SNWindow
    {
        public static Rect CreateWindow(Rect windowRect, object title, bool isTimeLeft = false, bool darkerBackground = false)
        {
            int titleHeight = Screen.height / 45;

            GUI.Box(windowRect, "");

            if (darkerBackground)
            {
                GUI.Box(windowRect, "");
            }

            if (title != null)
            {
                GUI.Box(new Rect(windowRect.x, windowRect.y, windowRect.width, titleHeight), "");

                if (isTimeLeft)
                {
                    GUI.Label(new Rect(windowRect.x + 5, windowRect.y, windowRect.width * 0.85f, titleHeight), title.ToString());
                    GUI.Label(new Rect(windowRect.x + windowRect.width * 0.85f, windowRect.y, windowRect.width, titleHeight), DateTime.Now.ToString("HH:mm:ss"));
                }
                else
                {
                    GUI.Label(new Rect(windowRect.x + 5, windowRect.y, windowRect.width, titleHeight), title.ToString());
                }

                return new Rect(windowRect.x, windowRect.y + titleHeight, windowRect.width, windowRect.height - titleHeight);
            }

            return windowRect;
        }

        public static Rect InitWindowRect(Rect windowRect, bool withTitle = true)
        {                         
            if (withTitle)
            {
                int titleHeight = Screen.height / 45;
                return new Rect(windowRect.x, windowRect.y + titleHeight, windowRect.width, windowRect.height - titleHeight);
            }

            return windowRect;
        }

        public static Rect[] SetGridItemsRect(Rect rect, int columns, int rows, int space, int itemHeight, bool alignRightDown = true, bool labeled = false)
        {
            if (columns < 1 || rows < 1)
            {
                throw new ArgumentException("The number of rows or columns must not be less than one!");
            }

            if (((rows * itemHeight) + rows * space) > rect.height)
            {
                throw new ArgumentException("The size of the elements is larger than window height!");
            }

            float calcWidth = (rect.width - ((columns + 1) * space)) / columns;

            int items = columns * rows;
            
            if (labeled)
            {                
                rect.y = rect.y + (itemHeight - space / 2);
            }
            
            Rect[] rects = new Rect[items];

            int row = 0;
            int column = 0;

            for (int i = 0; i < items; i++)
            {
                rects[i] = new Rect(rect.x + space + (column * (calcWidth + space)), rect.y + space + (row * (itemHeight + space)), calcWidth, itemHeight);

                if (alignRightDown && columns > 1)
                {
                    if (column == columns)
                    {
                        column = 0;
                        row++;
                        continue;
                    }
                    column++;
                }
                else
                {
                    if (row == rows)
                    {
                        row = 0;
                        column++;
                        continue;
                    }
                    row++;
                }                
            }
            
            if (labeled)
            {
                Array.Resize(ref rects, items + 1);
                rect.y = rect.y - (itemHeight - space / 2);
                rects[items] = new Rect(rect.x + space / 2, rect.y + space / 2, rect.width, itemHeight);
            }
            
            return rects;
        }

        public static int GetGridItemInt(int requiredColumn, int requiredRow, int columns, int rows, bool alignRightDown = true)
        {
            if (requiredColumn < 1 || requiredColumn > columns || requiredRow < 1 || requiredRow > rows)
            {
                throw new ArgumentException("The requested position is out of range!");
            }

            int row = 1;
            int column = 1;

            for (int i = 0; i < (columns * rows); i++)
            {
                if (row == requiredRow && column == requiredColumn)
                {
                    return i;
                }

                if (alignRightDown && columns > 1)
                {
                    if (column == columns)
                    {
                        column = 1;
                        row++;
                        continue;
                    }
                    column++;
                }
                else
                {
                    if (row == rows)
                    {
                        row = 1;
                        column++;
                        continue;
                    }
                    row++;
                }
            }

            return -1;
        }

        public static Rect GetGridItemRect(ref Rect[] gridItems, int requiredColumn, int requiredRow, int columns, int rows, bool alignRightDown = true)
        {
            if (requiredColumn < 1 || requiredColumn > columns || requiredRow < 1 || requiredRow > rows)
            {
                throw new ArgumentException("The requested position is out of range!");
            }

            int row = 1;
            int column = 1;

            for (int i = 0; i < (columns * rows); i++)
            {
                if (row == requiredRow && column == requiredColumn)
                {
                    return gridItems[i];
                }

                if (alignRightDown && columns > 1)
                {
                    if (column == columns)
                    {
                        column = 1;
                        row++;
                        continue;
                    }
                    column++;
                }
                else
                {
                    if (row == rows)
                    {
                        row = 1;
                        column++;
                        continue;
                    }
                    row++;
                }
            }

            throw new Exception("Unknown error!");
        }
    }
}
