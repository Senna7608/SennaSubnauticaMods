using Common;
using HarmonyLib;

namespace SlotExtender.Patches
{
    [HarmonyPatch(typeof(UpgradeConsole))]
    [HarmonyPatch("UnlockDefaultModuleSlots")]
    internal class UpgradeConsole_UnlockDefaultModuleSlots_Patch
    {
        [HarmonyPostfix]
        internal static void Postfix(UpgradeConsole __instance)
        {
            __instance.modules.AddSlots(SlotHelper.SessionNewCyclopsSlotIDs);

            SNLogger.Log("Cyclops Upgrade Slots Patched.");            
        }
    }
}
