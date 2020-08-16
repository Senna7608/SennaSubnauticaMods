using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using QModManager.API.ModLoading;

namespace AncientSword
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
                new SwordPrefab().Patch();

                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(),  "Subnautica.AncientSword.mod");                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }            
    }
}
