using HarmonyLib;
using QModManager.API.ModLoading;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ModdedArmsHelper
{
    [QModCore]
    public static class Main
    {
        internal static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        internal static GameObject helperRoot = null;
        internal static GameObject graphicsRoot = null;
        internal static ArmsGraphics graphics = null;        

        [QModPatch]
        public static void Load()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                Harmony.CreateAndPatchAll(assembly, $"Subnautica.{assembly.GetName().Name}.mod");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
