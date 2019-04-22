using Harmony;
using Common;
using SlotExtender.Configuration;

namespace SlotExtender.Patchers
{    
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("slotIDs", MethodType.Getter)]
    internal class Seamoth_slotIDs_Patch
    {
        [HarmonyPrefix]
        internal static bool Prefix(ref string[] __result)
        {
            //__result = SlotHelper.ExpandedSeamothSlotIDs;
            __result = SlotHelper.SessionSeamothSlotIDs;
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
            __instance.gameObject.AddIfNeedComponent<SlotExtender>();
            SNLogger.Log($"[{Config.PROGRAM_NAME}] Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");
        }
    }
}
