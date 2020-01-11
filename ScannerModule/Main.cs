using System;
using System.Reflection;
using Harmony;
using UnityEngine;
using Common;

namespace ScannerModule
{
    public static class Main
    {
        public static void Load()
        {
            try
            {
                var scannermodule = new ScannerModulePrefab();
                scannermodule.Patch();

                HarmonyInstance.Create("Subnautica.ScannerModule.mod").PatchAll(Assembly.GetExecutingAssembly());                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }    
}
