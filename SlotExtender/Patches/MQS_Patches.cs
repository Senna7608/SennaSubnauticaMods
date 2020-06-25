using System;
using Harmony;
using UnityEngine;
using SlotExtender.Configuration;
using Common;

namespace SlotExtender.Patches
{
    internal static class MQS_Patches
    {
        internal static bool InitPatch(HarmonyInstance hInstance)
        {
            try
            {
                if (RefHelp.SafeGetTypeFromAssembly("MoreQuickSlots", "MoreQuickSlots.GameController") is Type typeGameController)
                {
                    hInstance.Patch(AccessTools.Method(typeGameController, "CreateNewText"),
                        new HarmonyMethod(typeof(MQS_Patches), nameof(MQS_GameController_CreateNewText_Prefix)));
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }

            return true;
        }

        private static void MQS_GameController_CreateNewText_Prefix(ref string newText, int index)
        {
            if (index >= SEConfig.SLOTKEYSLIST.Count || Player.main.GetPDA().state == PDA.State.Opening)
                return;

            if (Player.main.inSeamoth)
            {
                newText = SEConfig.SLOTKEYSLIST[index];
            }
            else if (Player.main.inExosuit)
            {
                newText = index < 2? "": SEConfig.SLOTKEYSLIST[index - 2];
            }
        }
    }
}