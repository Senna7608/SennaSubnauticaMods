using System;
using Harmony;

namespace SlotExtender.Patchers
{
    [HarmonyPatch(typeof(Equipment))]
    [HarmonyPatch("GetSlotType")]
    internal class Equipment_GetSlotType_Patch
    {
        [HarmonyPrefix]
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
        [HarmonyPrefix]
        internal static bool Prefix(Equipment __instance, string slot, Pickupable pickupable, bool verbose, ref bool __result)
        {
            bool notAllowedInExtendedSlots = pickupable.GetTechType() == TechType.VehicleStorageModule ||
                                             pickupable.GetTechType() == TechType.SeamothTorpedoModule;

            if (notAllowedInExtendedSlots && __instance.owner.GetComponent<SlotExtender>().Instance.IsExtendedSeamothSlot(slot))
            {
                // Do not allow storage and torpedo modules in extended slots in Seamoth
                __result = false;
                ErrorMessage.AddMessage("Slot Extender:\nStorage module not allowed for this slot!");
                return false;
            }            
            else if (pickupable.GetTechType() == TechType.VehicleStorageModule && __instance.owner.name.Equals("Exosuit(Clone)"))
            {                
                // Do not allow more than four storage modules in Exosuit slots                               
                if (__instance.GetCount(TechType.VehicleStorageModule) >= 4)
                {
                    __result = false;
                    ErrorMessage.AddMessage("Slot Extender:\nStorage module limit reached!");
                    return false;
                }
            }
            return true;            
        }        
    }   
}
