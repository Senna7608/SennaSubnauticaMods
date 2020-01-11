using Common;
using Harmony;
using System;
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
                if (__instance.armPrefabs[i].techType == Main.techTypeID)
                {
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
                        prefab = __instance.armPrefabs[i].prefab.GetPrefabClone(null, true, "ExosuitPlasmaCannonArm"),
                        techType = Main.techTypeID
                    };                    
                    break;
                }
            }
            
            GameObject PlasmaCannonPrefab = __instance.armPrefabs[arraySize].prefab;

            PlasmaCannonPrefab.transform.position = new Vector3(0, -1930, 0);

            UnityEngine.Object.DestroyImmediate(PlasmaCannonPrefab.GetComponent<ExosuitTorpedoArm>());

            GameObject elbow = PlasmaCannonPrefab.FindChildInMaxDepth("elbow");                   

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

            GameObject ArmRig = PlasmaCannonPrefab.FindChildInMaxDepth("ArmRig");            ;            

            GameObject exosuit_arm_plasmaCannon_geo = ArmRig.FindChild("exosuit_arm_torpedoLauncher_geo");            

            exosuit_arm_plasmaCannon_geo.name = "exosuit_arm_plasmaCannon_geo";           

            exosuit_arm_plasmaCannon_geo.SetMeshMaterial(Main.cannon_material, 1);

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

            PlasmaCannonArmControl control = PlasmaCannonPrefab.GetOrAddComponent<PlasmaCannonArmControl>();

            GameObject shootSFX = Main.plasmaShootSFX.GetPrefabClone(plasmaCannon.transform, true);                      
            
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
            if (techType == Main.techTypeID)
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
                if (!KnownTech.Contains(Main.techTypeID))
                {
                    KnownTech.Add(Main.techTypeID);
                    ErrorMessage.AddMessage("P.R.A.W.N Plasma Cannon Arm blueprint discovered!");
                }
            }
            return true;
        }
    }
}
