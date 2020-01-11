using System;
using UnityEngine;
using Harmony;
using System.Reflection;
using SMLHelper.V2.Utility;

namespace RepulsionCannonArm
{
    public static class Main
    {
        public static Texture2D illumTex;

        public static void Load()
        {
            try
            {                
                var repulsionCannonArm = new RepulsionCannonArmPrefab();

                repulsionCannonArm.Patch();
                
                illumTex = ImageUtils.LoadTextureFromFile("./QMods/RepulsionCannonArm/Assets/Exosuit_Arm_Repulsion_Cannon_illum.png");

                HarmonyInstance.Create("Subnautica.ExosuitRepulsionCannonArm.mod").PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }          
    }    
    
}
