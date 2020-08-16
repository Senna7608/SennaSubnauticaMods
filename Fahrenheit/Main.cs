using System;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using QModManager.API.ModLoading;

namespace MAC.Fahrenheit
{
    [QModCore]
    public static class Main
    {
        [QModPatch]
        public static void Load()
        {
            try
            {
                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "com.MAC.Fahrenheit");                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
