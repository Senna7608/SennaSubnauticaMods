using System;
using UnityEngine;

namespace Common
{
    public enum COLORS
    {
        Red,
        Green,
        Blue,
        Yellow,
        White,
        Magenta,
        Cyan,
        Orange,
        Lime,
        Amethyst,
        Default
    }

    public static class Modules
    {
        public struct Colors
        {

            public static Color Red = Color.red;
            public static Color Green = Color.green;
            public static Color Blue = Color.blue;
            public static Color Yellow = Color.yellow;
            public static Color White = Color.white;
            public static Color Magenta = Color.magenta;
            public static Color Cyan = Color.cyan;
            public static Color Orange = new Color(1f, 0.647f, 0f, 1f);
            public static Color Lime = new Color(0.749f, 1f, 0f, 1f);
            public static Color Amethyst = new Color(0.6f, 0.4f, 0.8f, 1f);
            public static Color Default = new Color (0.338f, 0.475f, 0.978f, 1f);           

            public static Color[] ColorArray { get; } = { Red, Green, Blue, Yellow, White, Magenta, Cyan, Orange, Lime, Amethyst, Default};
            public static string[] ColorNames { get; } = { "Red", "Green", "Blue", "Yellow", "White", "Magenta", "Cyan", "Orange", "Lime", "Amethyst", "Default" };
            
        }

        public static Color GetColor(COLORS color)
        {
            return Colors.ColorArray[(int)color];
        }

        public static Color GetColor(string color)
        {
            int result = Array.IndexOf(Colors.ColorNames, color);

            if (result < 0)
                return Color.white;

            return Colors.ColorArray[result];
        }

        public static string GetColorName(Color color)
        {
            for (int i = 0; i < Colors.ColorArray.Length; i++)
            {
                if (Colors.ColorArray[i].Equals(color))
                    return Colors.ColorNames[i];
            }

            return null;
        }
    }
}
