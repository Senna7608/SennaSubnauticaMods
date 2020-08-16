using System;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using SeamothArms.ArmPrefabs;
using SMLHelper.V2.Handlers;
using QModManager.API.ModLoading;

namespace SeamothArms
{
    [QModCore]
    public static class Main
    {
        public static SeamothArms_Graphics graphics;

        public static bool isGraphicsReady = false;

        [QModPatch]
        public static void Load()
        {
            try
            {
                if (!isGraphicsReady)
                {
                    graphics = new SeamothArms_Graphics();
                }

                new SeamothDrillArmPrefab().Patch();
                new SeamothClawArmPrefab().Patch();                
                new SeamothGrapplingArmPrefab().Patch();
                new SeamothPropulsionArmPrefab().Patch();
                new SeamothTorpedoArmPrefab().Patch();

                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "Subnautica.SeamothArms.mod");                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }       
    }    
    
}
