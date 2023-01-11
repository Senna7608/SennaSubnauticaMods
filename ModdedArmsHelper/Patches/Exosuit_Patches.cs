using UnityEngine;
using HarmonyLib;
using ModdedArmsHelper.API;

namespace ModdedArmsHelper.Patches
{
    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("GetArmPrefab")]
    internal class Exosuit_GetArmPrefab_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Exosuit __instance, TechType techType, ref GameObject __result)
        {
            if (Main.armsGraphics.ModdedArmPrefabs.TryGetValue(techType, out GameObject moddedArmPrefab))
            {
                __result = moddedArmPrefab;
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("SpawnArm")]
    internal class Exosuit_SpawnArm_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Exosuit __instance, TechType techType, Transform parent, ref IExosuitArm __result)
        {
            if (Main.armsGraphics.ModdedArmPrefabs.ContainsKey(techType))
            {
                __result.GetGameObject().SetActive(true);                
            }            
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("OnUpgradeModuleChange")]
    internal class Exosuit_OnUpgradeModuleChange_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Exosuit __instance, int slotID, TechType techType, bool added)
        {
            if (Main.armsGraphics.ModdedArmPrefabs.ContainsKey(techType))
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

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("UpdateColliders")]
    internal class Exosuit_UpdateColliders_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Exosuit __instance)
        {
            __instance.BroadcastMessage("ResetColors", SendMessageOptions.DontRequireReceiver);
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("UpdateUIText")]
    internal class Exosuit_UpdateUIText_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Exosuit __instance, ref bool hasPropCannon)
        {
           IExosuitArm exosuitArmLeft = __instance.leftArmAttach.GetComponentInChildren<IExosuitArm>();

            if (exosuitArmLeft.GetGameObject().TryGetComponent(out ArmTag armTagLeft))
            {
                if (armTagLeft.armTemplate == ArmTemplate.PropulsionArm)
                {
                    hasPropCannon = true;
                    return true;
                }
            }

            IExosuitArm exosuitArmRight = __instance.rightArmAttach.GetComponentInChildren<IExosuitArm>();

            if (exosuitArmRight.GetGameObject().TryGetComponent(out ArmTag armTagRight))
            {
                if (armTagRight.armTemplate == ArmTemplate.PropulsionArm)
                {
                    hasPropCannon = true;
                    return true;
                }
            }

            return false;
        }
    }
}
