using System;
using UnityEngine;

namespace Common
{
    public static class UnityHelper
    {
        public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
        {
            if (!(gameObject.GetComponent<T>() == null))
            {
                return gameObject.GetComponent<T>();
            }
            return gameObject.AddComponent<T>();
        }

        public static Component AddOrGetComponent(this GameObject gameObject, Type component)
        {
            if (!(gameObject.GetComponent(component) == null))
            {
                return gameObject.GetComponent(component);
            }
            return gameObject.AddComponent(component);
        }

        public static bool AddIfNeedComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() == null)
            {
                gameObject.AddComponent<T>();
                return true;
            }
            return false;           
        }

        public static bool AddIfNeedComponent(this GameObject gameObject, Type component)
        {
            if (gameObject.GetComponent(component) == null)
            {
                gameObject.AddComponent(component);
                return true;
            }
            return false;
        }

        public static void EnableConsole()
        {
            DevConsole.disableConsole = false;
        }        
    }
}
