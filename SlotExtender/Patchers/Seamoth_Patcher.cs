using Harmony;
using UnityEngine;

namespace SlotExtender.Patchers
{
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("slotIDs", PropertyMethod.Getter)]
    internal class SeaMoth_slotIDs_Patch
    {
        internal static bool Prefix(ref string[] __result)
        {
            __result = SlotHelper.ExpandedSeamothSlotIDs;
            return false;
        }
    }

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Awake")]
    internal class SeaMoth_Awake_Patch
    {
        internal static void Postfix(SeaMoth __instance)
        {
            if (__instance.GetComponent<SlotExtender>() == null)
            {
                __instance.gameObject.AddComponent<SlotExtender>();

                Debug.Log($"[SlotExtender] Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");
            }
        }
    }
}
