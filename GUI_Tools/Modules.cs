using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Common.Modules
{
    public static class Modules
    {
        public static class Colors
        {
            public static Color White = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            public static Color Red = new Color32(byte.MaxValue, byte.MinValue, byte.MinValue, byte.MaxValue);
            public static Color Green = new Color32(byte.MinValue, byte.MaxValue, byte.MinValue, byte.MaxValue);
            public static Color Blue = new Color32(byte.MinValue, byte.MinValue, byte.MaxValue, byte.MaxValue);
            public static Color Yellow = new Color32(byte.MaxValue, byte.MaxValue, byte.MinValue, byte.MaxValue);
            public static Color Orange = new Color32(255, 128, byte.MinValue, byte.MaxValue);
        }

        public static void SetProgressColor(Color color)
        {
            HandReticle.main.progressText.color = color;
            HandReticle.main.progressImage.color = color;
        }

        public static void SetInteractColor(Color color, bool isSetSecondary = true)
        {
            HandReticle.main.interactPrimaryText.color = color;

            if (isSetSecondary)
                HandReticle.main.interactSecondaryText.color = color;
        }

        public static void PrintObject(GameObject obj, string indent = "", bool includeMaterials = false)
        {
            if (obj == null)
            {
                Debug.Log(indent + "null");
                return;
            }
            Debug.Log(indent + "[[" + obj.name + "]]:");
            Debug.Log(indent + "{");
            Debug.Log(indent + "  Components:");
            Debug.Log(indent + "  {");
            var lastC = obj.GetComponents<Component>().Last();
            foreach (var c in obj.GetComponents<Component>())
            {
                Debug.Log(indent + "    (" + c.GetType().ToString() + ")");
                if (includeMaterials)
                {
                    if (c.GetType().IsAssignableFrom(typeof(SkinnedMeshRenderer)) || c.GetType().IsAssignableFrom(typeof(MeshRenderer)))
                    {
                        var renderer = c as Renderer;
                        Debug.Log(indent + "    {");
                        foreach (var material in renderer.materials)
                        {
                            Debug.Log(indent + $"      {material}");
                        }
                        Debug.Log(indent + "    }");
                        Debug.Log(indent + "    {");
                        foreach (var material in renderer.sharedMaterials)
                        {
                            Debug.Log(indent + $"      {material}");
                        }
                        Debug.Log(indent + "    }");
                    }
                }
            }
            Debug.Log(indent + "  }");
            Debug.Log(indent + "  Children:");
            Debug.Log(indent + "  {");
            foreach (Transform child in obj.transform)
            {
                PrintObject(child.gameObject, indent + "    ");
            }
            Debug.Log(indent + "  }");
            Debug.Log(indent + "}");
        }

        public static void PrintObjectFields(object obj, string indent = "")
        {
            if (obj == null)
            {
                Debug.Log(indent + "  null");
                return;
            }

            Type type = obj.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                Debug.Log(indent + "  " + field.Name + " : " + field.GetValue(obj));
            }
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            foreach (PropertyInfo property in properties)
            {
                Debug.Log(indent + "  " + property.Name + " : " + property.GetValue(obj, new object[] { }));
            }
        }




    }
}
