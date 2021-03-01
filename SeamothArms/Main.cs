using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using QModManager.API.ModLoading;
using SeamothArms.ArmPrefabs;

namespace SeamothArms
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
                SeamothClawArmFragmentPrefab seamothClawArmFragment = new SeamothClawArmFragmentPrefab();
                seamothClawArmFragment.Patch();
                new SeamothClawArmPrefab(seamothClawArmFragment).Patch();
                                
                SeamothDrillArmFragmentPrefab seamothDrillArmFragment = new SeamothDrillArmFragmentPrefab();
                seamothDrillArmFragment.Patch();
                new SeamothDrillArmPrefab(seamothDrillArmFragment).Patch();                
                new SeamothGrapplingArmPrefab(null).Patch();
                new SeamothPropulsionArmPrefab(null).Patch();                
                new SeamothTorpedoArmPrefab(null).Patch();               
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }       
    }    
    
}