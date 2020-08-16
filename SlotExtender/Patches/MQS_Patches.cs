using System;
using UnityEngine;
using HarmonyLib;
using SlotExtender.Configuration;

namespace SlotExtender.Patches
{
    internal static class MQS_Patches
    {
        internal static bool InitPatch(Harmony hInstance)
        {
            try
            {
                if (Type.GetType("MoreQuickSlots.GameController, MoreQuickSlots", false) is Type typeGameController)
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
            if (index >= SEConfig.SLOTKEYBINDINGS.Count || Player.main.GetPDA().state == PDA.State.Opening)
                return;

            if (Player.main.inSeamoth)
            {
                newText = SlotHelper.SessionSeamothSlots[index].KeyCodeName;
            }
            else if (Player.main.inExosuit)
            {
                newText = SlotHelper.SessionExosuitSlots[index].KeyCodeName;
            }
        }
    }
}