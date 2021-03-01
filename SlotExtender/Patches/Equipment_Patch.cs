using System;
using UnityEngine;
using HarmonyLib;
using SlotExtender.Configuration;
using Common;

namespace SlotExtender.Patches
{
    [HarmonyPatch(typeof(Equipment), "AllowedToAdd")]    
    [HarmonyPatch(new Type[] { typeof(string), typeof(Pickupable), typeof(bool) })]
    internal class Equipment_AllowedToAdd_Patch
    {
        [HarmonyPrefix]
        internal static bool Prefix(Equipment __instance, string slot, Pickupable pickupable, bool verbose, ref bool __result)
        {
            TechType techTypeInSlot = pickupable.GetTechType();

            bool isExtendedSeamothSlot = SlotHelper.IsExtendedSeamothSlot(slot);

            if (isExtendedSeamothSlot && techTypeInSlot == TechType.SeamothTorpedoModule)
            {
                // Do not allow torpedo modules in extended slots in Seamoth
                __result = false;
                ErrorMessage.AddDebug("Slot Extender:\nTorpedo module not allowed for this slot!");
                return false;
            }

            if (techTypeInSlot == TechType.VehicleStorageModule)
            {
                if (__instance.owner.GetComponent<Exosuit>())
                {
                    // Do not allow more than four storage modules in Exosuit slots
                    if (__instance.GetCount(TechType.VehicleStorageModule) >= 4)
                    {
                        __result = false;
                        ErrorMessage.AddDebug("Slot Extender:\nStorage module limit reached!");
                        return false;
                    }
                }
                else if (SEConfig.STORAGE_SLOTS_OFFSET == 0)
                {
                    if (!isExtendedSeamothSlot)
                        return true;

                    // Do not allow storage modules in extended slots in Seamoth
                    __result = false;
                    ErrorMessage.AddDebug("Slot Extender:\nStorage module not allowed for this slot!");
                    return false;
                }
                else if (__instance.owner.GetComponent<SeaMoth>() is SeaMoth seamoth)
                {
                    int slotID = int.Parse(slot.Substring(13)) - 1;

                    if (slotID > 3 && (slotID < SEConfig.STORAGE_SLOTS_OFFSET || slotID > SEConfig.STORAGE_SLOTS_OFFSET + 3))
                    {
                        ErrorMessage.AddDebug("Slot Extender:\nStorage module not allowed for this slot!");
                        return false;
                    }

                    // HACK: trying to swap one storage to another while drag, silently refusing because of ui problems
                    if (seamoth.GetSlotItem(slotID)?.item.GetTechType() == TechType.VehicleStorageModule)
                    {
                        __result = false;
                        return false;
                    }

                    SeamothStorageInput storageInput = seamoth.storageInputs[slotID % SEConfig.STORAGE_SLOTS_OFFSET];
                    var fieldState = AccessTools.Field(typeof(SeamothStorageInput), "state");
                    __result = !(bool)fieldState.GetValue(storageInput); //already active

                    if (!__result && verbose)
                    {
                        int _slotID = (slotID < 4? slotID + SEConfig.STORAGE_SLOTS_OFFSET: slotID - SEConfig.STORAGE_SLOTS_OFFSET) + 1;
                        ErrorMessage.AddDebug($"Slot Extender:\nStorage module is already in the slot {_slotID}");
                    }

                    return false;
                }
            }

            return true;
        }
    }
}
