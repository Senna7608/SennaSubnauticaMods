using HarmonyLib;

namespace SeamothArms.Patches
{
    [HarmonyPatch(typeof(Drillable))]
    [HarmonyPatch("Start")]
    public class Drillable_Start_Patch
    {
        static void Postfix(Drillable __instance)
        {            
             __instance.gameObject.EnsureComponent<SeamothDrillable>();                     
        }
    }

    [HarmonyPatch(typeof(Drillable))]
    [HarmonyPatch("HoverDrillable")]
    public class Drillable_HoverDrillable_Patch
    {
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
