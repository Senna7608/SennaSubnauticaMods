using Harmony;
using UnityEngine;

namespace SlotExtender.Patchers
{
    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("slotIDs", PropertyMethod.Getter)]
    internal class Exosuit_slotIDs_Patch
    {
        internal static bool Prefix(ref string[] __result)
        {
            __result = SlotHelper.ExpandedExosuitSlotIDs;
            return false;
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("Awake")]
    internal class Exosuit_Awake_Patch
    {
        internal static void Postfix(Exosuit __instance)
        {
            if (__instance.GetComponent<SlotExtender>() == null)
            {
                __instance.gameObject.AddComponent<SlotExtender>();

                Debug.Log($"[SlotExtender] Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");
            }
        }
    }
}
