using HarmonyLib;
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
    
    [HarmonyPatch(typeof(Exosuit), "Awake")]    
    public class Exosuit_Awake_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Exosuit __instance)
        {            
            __instance.gameObject.EnsureComponent<SlotExtender>();
            SNLogger.Debug($"Component added in Exosuit.Awake -> Postfix Patch. ID: {__instance.GetInstanceID()}");           
        }
    }
}
