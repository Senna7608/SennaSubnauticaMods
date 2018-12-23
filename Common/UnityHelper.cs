using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Common
{
    public static class UnityHelper
    {
        public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() != null)
            {
                return gameObject.GetComponent<T>();
            }
            else
            {
                return gameObject.AddComponent<T>();
            }
        }

        public static void EnableConsole()
        {
            DevConsole.disableConsole = false;
        }
              





    }
}
