using Common;
using Harmony;
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
                        prefab = __instance.armPrefabs[i].prefab.GetPrefabClone(null, true, "ExosuitRepulsionCannonArm"),
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

            GameObject elbow = RepulsionCannonPrefab.FindChildInMaxDepth("elbow");            

            GameObject repulsion = elbow.FindChild("propulsion");            

            repulsion.name = "repulsion";
                        
            GameObject ArmRig = RepulsionCannonPrefab.FindChildInMaxDepth("ArmRig");            

            GameObject exosuit_repulsion_geo = ArmRig.FindChild("exosuit_propulsion_geo");           

            exosuit_repulsion_geo.name = "exosuit_repulsion_geo";

            exosuit_repulsion_geo.ChangeObjectTexture(2, illumTex: Main.illumTex.GetObjectClone());            

            RepulsionCannonArmControl control = RepulsionCannonPrefab.GetOrAddComponent<RepulsionCannonArmControl>();

            control.muzzle = RepulsionCannonPrefab.FindChildInMaxDepth("repulsion").FindChild("muzzle").transform;

            var repulsionPrefab = Resources.Load<GameObject>("WorldEntities/Tools/RepulsionCannon").GetPrefabClone();            

            var repulsionCannon = repulsionPrefab.GetComponent<RepulsionCannon>();                        

            control.fxControl = repulsionCannon.fxControl.GetComponentClone(control.transform);

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

            control.bubblesFX = repulsionCannon.bubblesFX.GetPrefabClone(RepulsionCannonPrefab.transform, false);
            
            control.shootSound = repulsionCannon.shootSound.GetObjectClone();

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
