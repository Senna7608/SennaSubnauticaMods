using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using SMLHelper.V2.Utility;
using Common.Helpers;
using QModManager.API.ModLoading;

namespace RepulsionCannonArm
{
    [QModCore]
    public static class Main
    {
        public static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static ObjectHelper objectHelper = new ObjectHelper();        

        public static Texture2D illumTex;

        [QModPatch]
        public static void Load()
        {
            try
            {
                new RepulsionCannonArmPrefab().Patch();
                
                illumTex = ImageUtils.LoadTextureFromFile($"{modFolder}/Assets/Exosuit_Arm_Repulsion_Cannon_illum.png");

                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "Subnautica.ExosuitRepulsionCannonArm.mod");                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }          
    }    
    
}
