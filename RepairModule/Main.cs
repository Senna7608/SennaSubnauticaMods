using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using UnityEngine;

namespace RepairModule
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
                new RepairModulePrefab().Patch();

                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "Subnautica.RepairModule.mod");                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }       
}
