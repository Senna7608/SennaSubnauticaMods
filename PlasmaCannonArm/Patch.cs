using Common;
using Common.Helpers;
using HarmonyLib;
using System;
using System.Diagnostics;
using UnityEngine;

namespace PlasmaCannonArm
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
                if (__instance.armPrefabs[i].techType == PlasmaCannonArmPrefab.TechTypeID)
                {
                    SNLogger.Debug("PlasmaCannonArm", "TechType already found on Exosuit.armPrefabs array. Skipping patch.");
                    return;
                }
            } 
            
            for (int i = 0; i < arraySize; i++)
            {
                if (__instance.armPrefabs[i].techType == TechType.ExosuitTorpedoArmModule)
                {
                    Array.Resize(ref __instance.armPrefabs, arraySize + 1);

                    __instance.armPrefabs[arraySize] = new Exosuit.ExosuitArmPrefab()
                    {
                        prefab = Main.objectHelper.GetPrefabClone(__instance.armPrefabs[i].prefab, null, true, "ExosuitPlasmaCannonArm"),
                        techType = PlasmaCannonArmPrefab.TechTypeID
                    };
                    
                    break;
                }
            }
            
            GameObject PlasmaCannonPrefab = __instance.armPrefabs[arraySize].prefab;

            SNLogger.Debug("PlasmaCannonArm", $"Prefab created and added to armPrefabs array, name: [{PlasmaCannonPrefab.name}]");

            PlasmaCannonPrefab.transform.position = new Vector3(0, -1930, 0);

            UnityEngine.Object.DestroyImmediate(PlasmaCannonPrefab.GetComponent<ExosuitTorpedoArm>());

            GameObject elbow = Main.objectHelper.FindDeepChild(PlasmaCannonPrefab, "elbow");

            GameObject plasmaCannon = elbow.FindChild("torpedoLauncher");            

            plasmaCannon.name = "plasmaCannon";

            UnityEngine.Object.DestroyImmediate(plasmaCannon.FindChild("reload"));
            UnityEngine.Object.DestroyImmediate(plasmaCannon.FindChild("torpedo_innr1"));
            UnityEngine.Object.DestroyImmediate(plasmaCannon.FindChild("torpedo_innr2"));
            UnityEngine.Object.DestroyImmediate(plasmaCannon.FindChild("torpedo_outr1"));
            UnityEngine.Object.DestroyImmediate(plasmaCannon.FindChild("torpedo_outr2"));
            UnityEngine.Object.DestroyImmediate(plasmaCannon.FindChild("TorpedoFirst"));
            UnityEngine.Object.DestroyImmediate(plasmaCannon.FindChild("TorpedoSecond"));
            UnityEngine.Object.DestroyImmediate(plasmaCannon.FindChild("TorpedoReload"));
            UnityEngine.Object.DestroyImmediate(plasmaCannon.FindChild("collider"));

            GameObject PlasmaTubeRight = plasmaCannon.FindChild("TorpedoSiloFirst");
            PlasmaTubeRight.name = "PlasmaTubeRight";

            GameObject PlasmaTubeLeft = plasmaCannon.FindChild("TorpedoSiloSecond");
            PlasmaTubeLeft.name = "PlasmaTubeLeft";

            GameObject ArmRig = Main.objectHelper.FindDeepChild(PlasmaCannonPrefab, "ArmRig"); ;            

            GameObject exosuit_arm_plasmaCannon_geo = ArmRig.FindChild("exosuit_arm_torpedoLauncher_geo");            

            exosuit_arm_plasmaCannon_geo.name = "exosuit_arm_plasmaCannon_geo";           

            GraphicsHelper.SetMeshMaterial(exosuit_arm_plasmaCannon_geo,Main.cannon_material, 1);

            Renderer renderer = exosuit_arm_plasmaCannon_geo.GetComponent<Renderer>();

            Material[] materials = renderer.materials;

            materials[0].name = "exosuit_01";

            materials[1].name = "Exosuit_plasma_cannon_arm";            

            UnityEngine.Object.DestroyImmediate(exosuit_arm_plasmaCannon_geo.FindChild("torpedo_innr1_geo"));
            UnityEngine.Object.DestroyImmediate(exosuit_arm_plasmaCannon_geo.FindChild("torpedo_innr2_geo"));
            UnityEngine.Object.DestroyImmediate(exosuit_arm_plasmaCannon_geo.FindChild("torpedo_outr1_geo"));
            UnityEngine.Object.DestroyImmediate(exosuit_arm_plasmaCannon_geo.FindChild("torpedo_outr2_geo"));
            UnityEngine.Object.DestroyImmediate(exosuit_arm_plasmaCannon_geo.FindChild("torpedo_reload_geo"));

            Main.plasma_Material = Main.GetPlasmaMaterial();            

            PlasmaCannonArmControl control = PlasmaCannonPrefab.AddComponent<PlasmaCannonArmControl>();

            GameObject shootSFX = Main.objectHelper.GetPrefabClone(Main.plasmaShootSFX, plasmaCannon.transform, true);                      
            
            control.cannon_tube_left = PlasmaTubeLeft;
            control.cannon_tube_right = PlasmaTubeRight;
            control.audioSource = shootSFX.GetComponent<AudioSource>();            
        }        
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("OnUpgradeModuleChange")]
    public class Exosuit_OnUpgradeModuleChange_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Exosuit __instance, int slotID, TechType techType, bool added)
        {
            if (techType == PlasmaCannonArmPrefab.TechTypeID)
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

    [HarmonyPatch(typeof(PDAScanner))]
    [HarmonyPatch("Unlock")]
    public static class PDAScanner_Unlock_Patch
    {
        public static bool Prefix(PDAScanner.EntryData entryData)
        {
            if (entryData.key == TechType.PrecursorPrisonArtifact7)
            {
                if (!KnownTech.Contains(PlasmaCannonArmPrefab.TechTypeID))
                {
                    KnownTech.Add(PlasmaCannonArmPrefab.TechTypeID);
                    ErrorMessage.AddMessage("P.R.A.W.N Plasma Cannon Arm blueprint discovered!");
                }
            }
            return true;
        }
    }

    
    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("GetArmPrefab")]
    public class Exosuit_GetArmPrefab_Patch
    {        
        [HarmonyPrefix]
        [Conditional("DEBUG")]
        public static void Postfix(Exosuit __instance, TechType techType)
        {
            SNLogger.Debug("PlasmaCannonArm", $"Exosuit GetArmPrefab called, TechType request: [{techType.ToString()}]");

            SNLogger.Debug("PlasmaCannonArm", "Listing Exosuit.armPrefabs[]");

            foreach (Exosuit.ExosuitArmPrefab armPrefab in __instance.armPrefabs)
            {
                SNLogger.Debug("PlasmaCannonArm", $"armPrefab: [{armPrefab.prefab.name}]");
                SNLogger.Debug("PlasmaCannonArm", $"TechType: [{armPrefab.techType.ToString()}]");
            }
        }
    }

}
