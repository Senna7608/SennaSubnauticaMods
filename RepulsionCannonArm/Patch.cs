using Common.Helpers;
using HarmonyLib;
using System;
using UnityEngine;

namespace RepulsionCannonArm
{
    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("Start")]
    public class Exosuit_Start_Patch
    {
        [HarmonyPrefix]
        public static void Prefix(Exosuit __instance)
        {
            int arraySize = __instance.armPrefabs.Length;

            for (int i = 0; i < arraySize; i++)
            {
                if (__instance.armPrefabs[i].techType == RepulsionCannonArmPrefab.TechTypeID)
                {
                    return;
                }
            } 
            
            for (int i = 0; i < arraySize; i++)
            {
                if (__instance.armPrefabs[i].techType == TechType.ExosuitPropulsionArmModule)
                {
                    Array.Resize(ref __instance.armPrefabs, arraySize + 1);                    

                    __instance.armPrefabs[arraySize] = new Exosuit.ExosuitArmPrefab()
                    {
                        prefab = Main.objectHelper.GetPrefabClone(__instance.armPrefabs[i].prefab, null, true, "ExosuitRepulsionCannonArm"),
                        techType = RepulsionCannonArmPrefab.TechTypeID
                    };
                    
                    break;
                }
            }
            
            GameObject RepulsionCannonPrefab = __instance.armPrefabs[arraySize].prefab;

            RepulsionCannonPrefab.transform.position = new Vector3(0, -1930, 0);

            UnityEngine.Object.DestroyImmediate(RepulsionCannonPrefab.FindChild("xPropulsionCannon_Beams"));
            UnityEngine.Object.DestroyImmediate(RepulsionCannonPrefab.FindChild("PropulsionCannonGrabbedEffect"));

            UnityEngine.Object.DestroyImmediate(RepulsionCannonPrefab.GetComponent<ExosuitPropulsionArm>());
            UnityEngine.Object.DestroyImmediate(RepulsionCannonPrefab.GetComponent<PropulsionCannon>());

            GameObject elbow = Main.objectHelper.FindDeepChild(RepulsionCannonPrefab, "elbow");            

            GameObject repulsion = elbow.FindChild("propulsion");            

            repulsion.name = "repulsion";
                        
            GameObject ArmRig = Main.objectHelper.FindDeepChild(RepulsionCannonPrefab, "ArmRig");            

            GameObject exosuit_repulsion_geo = ArmRig.FindChild("exosuit_propulsion_geo");           

            exosuit_repulsion_geo.name = "exosuit_repulsion_geo";

            GraphicsHelper.ChangeObjectTexture(exosuit_repulsion_geo, 2, illumTex: Main.objectHelper.GetObjectClone(Main.illumTex));                       

            RepulsionCannonArmControl control = RepulsionCannonPrefab.AddComponent<RepulsionCannonArmControl>();

            control.muzzle = Main.objectHelper.FindDeepChild(RepulsionCannonPrefab, "muzzle").transform;            

            var repulsionPrefab = Main.objectHelper.GetPrefabClone(Resources.Load<GameObject>("WorldEntities/Tools/RepulsionCannon"));            

            var repulsionCannon = repulsionPrefab.GetComponent<RepulsionCannon>();                        

            control.fxControl = Main.objectHelper.GetComponentClone(repulsionCannon.fxControl, control.transform);

            for (int i = 0; i < control.fxControl.emitters.Length; i++)
            {
                VFXController.VFXEmitter emitter = control.fxControl.emitters[i];

                emitter.parentTransform = control.muzzle;

                if (emitter.fx != null)
                {
                    foreach (ParticleSystem ps in emitter.fx.GetComponentsInChildren<ParticleSystem>(true))
                    {
                        ParticleSystem.MainModule psMain = ps.main;

                        ParticleSystem.MinMaxCurve startSize = psMain.startSize;

                        if (startSize.mode == ParticleSystemCurveMode.TwoConstants)
                        {
                            startSize.constant = 1.5f;
                            startSize.constantMax = 1.5f;
                            startSize.constantMin = 1.5f;
                            psMain.startSize = startSize;
                        }
                    }
                }
            }

            control.bubblesFX = Main.objectHelper.GetPrefabClone(repulsionCannon.bubblesFX, RepulsionCannonPrefab.transform, false);
            
            control.shootSound = Main.objectHelper.GetObjectClone(repulsionCannon.shootSound);

            control.animator = RepulsionCannonPrefab.GetComponent<Animator>();

            UnityEngine.Object.DestroyImmediate(repulsionPrefab);
            
            UnityEngine.Object.DestroyImmediate(RepulsionCannonPrefab.FindChild("RepulsionPrefab(Clone)"));
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("OnUpgradeModuleChange")]
    public class Exosuit_OnUpgradeModuleChange_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Exosuit __instance, int slotID, TechType techType, bool added)
        {
            if (techType == RepulsionCannonArmPrefab.TechTypeID)
            {
                __instance.MarkArmsDirty();                
                return false;
            }
            else
            {                
                return true;
            }
        }
    }
    
}
