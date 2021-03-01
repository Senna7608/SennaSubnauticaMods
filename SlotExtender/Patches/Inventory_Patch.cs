using Common;
using HarmonyLib;

namespace SlotExtender.Patches
{
    [HarmonyPatch(typeof(Inventory))]
    [HarmonyPatch("UnlockDefaultEquipmentSlots")]
    internal class Inventory_UnlockDefaultEquipmentSlots_Patch
    {
        [HarmonyPostfix]
        internal static void Postfix(Inventory __instance)
        {
            __instance.equipment.AddSlots(SlotHelper.NewChipSlotIDs);

            SNLogger.Log("Inventory Chip Slots Patched.");            
        }
    }
}
