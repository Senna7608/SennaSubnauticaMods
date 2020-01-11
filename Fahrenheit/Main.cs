using System;
using UnityEngine;
using Harmony;
using System.Reflection;

namespace MAC.Fahrenheit
{
    public static class Main
    {
        public static void Load()
        {
            try
            {
                HarmonyInstance.Create("com.MAC.Fahrenheit").PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
