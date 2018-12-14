using System;
using Harmony;

namespace SlotExtender.Patchers
{
    [HarmonyPatch(typeof(Equipment))]
    [HarmonyPatch("GetSlotType")]
    internal class Equipment_GetSlotType_Patch
    {
        internal static void Prefix(string slot, ref EquipmentType __result)
        {
            SlotHelper.ExpandSlotMapping();
        }
    }

    [HarmonyPatch(typeof(Equipment))]
    [HarmonyPatch("AllowedToAdd")]
    [HarmonyPatch(new Type[] { typeof(string), typeof(Pickupable), typeof(bool) })]
    internal class Equipment_AllowedToAdd_Patch
    {
        internal static bool Prefix(Equipment __instance, string slot, Pickupable pickupable, bool verbose, ref bool __result)
        {
            if (pickupable.GetTechType() == TechType.VehicleStorageModule && __instance.owner.GetComponent<SlotExtender>().Instance.IsExtendedSeamothSlot(slot))
            {
                // Do not allow storage modules in extended slots
                __result = false;
                return false;
            }

            return true;
        }
    }
}
