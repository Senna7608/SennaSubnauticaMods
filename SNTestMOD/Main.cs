using System;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace SNTestMOD
{
    public static class Main
    {
        public static bool isAuroraRadiationBypassed = false;
        public static bool isPlayerInRadiationZone = false;

        public static void Load()
        {
            try
            {
                //TestPrefab testPrefab = new TestPrefab();
                //testPrefab.Patch();
                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "Subnautica.SNTestMOD.mod");                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        
        public static void PrintLOG(string message)
        {
            Debug.Log($"[SNTestMOD] {message}");
        }

    }    
    
}
