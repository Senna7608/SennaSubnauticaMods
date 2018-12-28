using Harmony;
using Common;

namespace SlotExtender.Patchers
{    
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("slotIDs", PropertyMethod.Getter)]
    internal class Seamoth_slotIDs_Patch
    {
        [HarmonyPrefix]
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
        [HarmonyPostfix]
        internal static void Postfix(SeaMoth __instance)
        {
            __instance.gameObject.AddIfNotComponent<SlotExtender>();
            Logger.Log($"Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");
        }
    }
}
