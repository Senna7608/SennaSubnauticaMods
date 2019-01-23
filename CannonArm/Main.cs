using System;
using System.Reflection;
using Harmony;
using UnityEngine;
using Common;

namespace CannonArm
{
    public static class Main
    {
        public static AssetBundle assetBundle;

        public static void Load()
        {
            try
            {
                assetBundle = AssetBundle.LoadFromFile("./QMods/CannonArm/Assets/exosuit_cannon_arm");

                if (assetBundle == null)
                    Debug.Log("[CannonArm] AssetBundle is NULL!");
                else
                    Debug.Log($"[CannonArm] AssetBundle loaded, name: {assetBundle.name}");



                var repairmodule = new CannonArm();
                repairmodule.Patch();

                HarmonyInstance.Create("Subnautica.CannonArm.mod").PatchAll(Assembly.GetExecutingAssembly());                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
    
    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("Awake")]     
    public class Exosuit_Awake_Patch
    {
        [HarmonyPrefix]
        static void Prefix(Exosuit __instance)
        {
            int arraySize = __instance.armPrefabs.Length;

            Array.Resize(ref __instance.armPrefabs, arraySize + 1);
            
            __instance.armPrefabs[arraySize] = new Exosuit.ExosuitArmPrefab()
            {
                prefab = CraftData.GetPrefabForTechType(CannonArm.TechTypeID),
                techType = CannonArm.TechTypeID
            };
            
            foreach (Exosuit.ExosuitArmPrefab item in __instance.armPrefabs)
            {
                if (item.techType == TechType.ExosuitTorpedoArmModule || item.techType == CannonArm.TechTypeID)
                Common.DebugHelper.DebugHelper.DebugGameObject(item.prefab);
            }
            
        }        
    }
    /*
    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("OnUpgradeModuleChange")]
    [HarmonyPatch(new Type[] { typeof(int), typeof(TechType), typeof(bool) })]
    public class Exosuit_OnUpgradeModuleChange_Patch
    {
        [HarmonyPostfix]
        static void Postfix(Exosuit __instance, int slotID, TechType techType, bool added)
        {            
             if (techType == TechType.ExosuitTorpedoArmModule)
                    Common.DebugHelper.DebugHelper.DebugGameObject(__instance.GetComponentInChildren<ExosuitTorpedoArm>().gameObject);           
        }
    }
    */
}
