using System;
using UnityEngine;
using Harmony;
using System.Reflection;
using SeamothArms.ArmPrefabs;

namespace SeamothArms
{
    public static class Main
    {
        public static void Load()
        {
            try
            {
                new SeamothDrillArmPrefab().Patch();
                new SeamothClawArmPrefab().Patch();                
                new SeamothGrapplingArmPrefab().Patch();
                new SeamothPropulsionArmPrefab().Patch();
                new SeamothTorpedoArmPrefab().Patch();                

                HarmonyInstance.Create("Subnautica.SeamothArms.mod").PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }        
    }    
    
}
