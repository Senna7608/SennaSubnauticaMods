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
            __result = SlotHelper.SessionSeamothSlotIDs;
            return false;
        }
    }    
    

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Start")]
    internal class SeaMoth_Start_Patch
    {
        [HarmonyPostfix]
        internal static void Postfix(SeaMoth __instance)
        {
            string name = __instance.name;
            int ID = __instance.GetInstanceID();

            __instance.gameObject.AddIfNeedComponent<SlotExtender>();

            SNLogger.Log($"[{SEConfig.PROGRAM_NAME}] component added to instance: {name} ID: {ID}");            
        }
    }
}
