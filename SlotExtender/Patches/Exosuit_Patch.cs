using Harmony;
using Common;

namespace SlotExtender.Patches
{
    [HarmonyPatch(typeof(Exosuit), "slotIDs", MethodType.Getter)]    
    public class Exosuit_slotIDs_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(ref string[] __result)
        {           
            __result = SlotHelper.SessionExosuitSlotIDs;
            return false;
        }
    }    
    
    [HarmonyPatch(typeof(Exosuit), "Start")]    
    public class Exosuit_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Exosuit __instance)
        {            
            __instance.gameObject.AddIfNeedComponent<SlotExtender>();
            SNLogger.Log("SlotExtender", $"Component added to instance: {__instance.name} ID: {__instance.GetInstanceID()}");           
        }
    }
}
