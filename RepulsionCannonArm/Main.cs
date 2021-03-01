using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using SMLHelper.V2.Utility;
using QModManager.API.ModLoading;

namespace RepulsionCannonArm
{
    [QModCore]
    public static class Main
    {
        public static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);                

        public static Texture2D illumTex;

        [QModPatch]
        public static void Load()
        {
            try
            {
                illumTex = ImageUtils.LoadTextureFromFile($"{modFolder}/Assets/Exosuit_Arm_Repulsion_Cannon_illum.png");

                new RepulsionCannonArmPrefab().Patch();                                             
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }          
    }    
}
