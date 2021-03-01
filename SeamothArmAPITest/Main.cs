using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using QModManager.API.ModLoading;
using SeamothArmAPITest.Prefabs;
using SeamothArmAPITest.Requesters;
using ModdedArmsHelper.API;

namespace SeamothArmAPITest
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
                NewClawArmPrefab newClawArm = new NewClawArmPrefab();
                newClawArm.Patch();
                ArmServices.main.RegisterArm(newClawArm, new NewClawArmHandlerRequest());

                NewDrillArmPrefab newDrillArm = new NewDrillArmPrefab();
                newDrillArm.Patch();
                ArmServices.main.RegisterArm(newDrillArm, new NewDrillArmHandlerRequest());

                NewGrapplingArmPrefab newGrapplingArm = new NewGrapplingArmPrefab();
                newGrapplingArm.Patch();
                ArmServices.main.RegisterArm(newGrapplingArm, new NewGrapplingArmHandlerRequest());

                NewPropulsionArmPrefab newPropulsionArm = new NewPropulsionArmPrefab();
                newPropulsionArm.Patch();
                ArmServices.main.RegisterArm(newPropulsionArm, new NewPropulsionArmHandlerRequest());

                NewTorpedoArmPrefab newTorpedoArm = new NewTorpedoArmPrefab();
                newTorpedoArm.Patch();
                ArmServices.main.RegisterArm(newTorpedoArm, new NewTorpedoArmHandlerRequest());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }                      
    }
}
