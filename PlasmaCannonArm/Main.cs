using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using QModManager.API.ModLoading;

namespace PlasmaCannonArm
{
    [QModCore]
    public static class Main
    {
        public static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static AssetBundle assetBundle;        
        public static Material plasma_Material;        

        [QModPatch]
        public static void Load()
        {
            try
            {                
                new PlasmaCannonArmPrefab().Patch();                

                assetBundle = AssetBundle.LoadFromFile($"{modFolder}/Assets/plasma_arm.asset");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }    
}
