using System;
using UnityEngine;
#pragma warning disable CS1591

namespace Common.Helpers
{
    public static class UnityHelper
    {
        public static bool IsRoot(this Transform transform)
        {
            return transform.parent == null ? true : false;
        }

        public static bool IsRoot(this GameObject gameObject)
        {
            return gameObject.transform.parent == null ? true : false;
        }        

        public static void CleanObject(this GameObject gameObject)
        {
            foreach (Component component in gameObject.GetComponents<Component>())
            {
                Type componentType = component.GetType();

                if (componentType == typeof(Transform))
                    continue;
                if (componentType == typeof(Renderer))
                    continue;
                if (componentType == typeof(Mesh))
                    continue;
                if (componentType == typeof(Shader))
                    continue;

                UnityEngine.Object.Destroy(component);
            }
        }

        public static string GetUeObjectShortType(this UnityEngine.Object ueObject)
        {
            return ueObject.GetType().ToString().Split('.').GetLast();
        }

        public static string GetPath(this Transform current)
        {
            if (current.parent == null)
                return current.name;
            return current.parent.GetPath() + "/" + current.name;
        }
    }
}

