using System;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using QModManager.API.ModLoading;
using System.IO;

namespace RotateMoonPool
{
    [QModCore]
    public static class Main
    {
        public static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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
