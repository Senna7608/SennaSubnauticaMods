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
            TechType techTypeInSlot = pickupable.GetTechType();
            
            if (SlotHelper.IsExtendedSeamothSlot(slot))
            {
                switch (techTypeInSlot)
                {
                    case TechType.VehicleStorageModule:
                        // Do not allow storage modules in extended slots in Seamoth
                        __result = false;
                        ErrorMessage.AddMessage("Slot Extender:\nStorage module not allowed for this slot!");
                        return false;
                        
                    case TechType.SeamothTorpedoModule:
                        // Do not allow torpedo modules in extended slots in Seamoth
                        __result = false;
                        ErrorMessage.AddMessage("Slot Extender:\nTorpedo module not allowed for this slot!");
                        return false;
                }

                return true;
            }
            
            if (techTypeInSlot == TechType.VehicleStorageModule && __instance.owner.name.Equals("Exosuit(Clone)"))
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
