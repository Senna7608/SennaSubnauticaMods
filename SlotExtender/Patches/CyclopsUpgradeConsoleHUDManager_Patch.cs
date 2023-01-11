using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SlotExtender.Patches
{
    internal static class CyclopsUpgradeConsoleHUDManager_Patch
    {
        [HarmonyPatch(typeof(CyclopsUpgradeConsoleHUDManager), "RefreshUpgradeConsoleIcons")]
        private static class CyclopsUpgradeConsoleHUDManager_RefreshUpgradeConsoleIcons_Patch
        {
            public static readonly Vector3 littleIcons = new Vector3(0.5f, 0.5f, 0.5f);
            //public static readonly Vector3 normalIcons = new Vector3(0.75f, 0.75f, 0.75f);

            [HarmonyPostfix]
            public static void Postfix(CyclopsUpgradeConsoleHUDManager __instance, TechType[] slottedTypes)
            {
                HorizontalLayoutGroup hlg = __instance.GetComponentInChildren<HorizontalLayoutGroup>(true);
                
                hlg.childScaleWidth = true;                

                foreach (Transform transform in __instance.icons)
                {                    
                    transform.localScale = littleIcons;                    
                }                
            }
        }
    }
}