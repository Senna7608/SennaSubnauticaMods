using System;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using Common.Helpers;
using System.IO;
using QModManager.API.ModLoading;

namespace ScannerModule
{
    [QModCore]
    public static class Main
    {
        public static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static ObjectHelper objectHelper { get; private set; }

        [QModPatch]
        public static void Load()
        {
            try
            {
                objectHelper = new ObjectHelper();

                new ScannerModulePrefab().Patch();

                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "Subnautica.ScannerModule.mod");               
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }    
}
