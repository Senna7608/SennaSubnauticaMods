using HarmonyLib;
using ModdedArmsHelper.API;

namespace ModdedArmsHelper.Patches
{
    [HarmonyPatch(typeof(Drillable))]
    [HarmonyPatch("Start")]
    internal class Drillable_Start_Patch
    {
        [HarmonyPostfix]
        static void Postfix(Drillable __instance)
        {            
             __instance.gameObject.EnsureComponent<SeamothDrillable>();                     
        }
    }

    [HarmonyPatch(typeof(Drillable))]
    [HarmonyPatch("HoverDrillable")]
    internal class Drillable_HoverDrillable_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Drillable __instance)
        {
            if (Player.main.inSeamoth)
            {
                __instance.GetComponent<SeamothDrillable>().HoverDrillable();

                return false;
            }

            return true;
        }
    }    
}
