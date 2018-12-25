using System;
using System.Reflection;
using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace SlotExtender.Patchers
{
    internal class MQS_Patcher
    {
        HarmonyInstance hInstance;

        public MQS_Patcher(HarmonyInstance hInstance)
        {
            this.hInstance = hInstance;
        }

        internal bool InitPatch()
        {
            try
            {
                hInstance.Patch(typeof(MoreQuickSlots.GameController).GetMethod("CreateNewText",
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Static), new HarmonyMethod(typeof(MQS_GameController_CreateNewText_Patch), "Prefix"), null);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(MoreQuickSlots.GameController))]    
    [HarmonyPatch("CreateNewText")]
    internal class MQS_GameController_CreateNewText_Patch
    {
        internal static void Prefix(ref string newText, int index)
        {
            List<string> slotkeys = new List<string>();
            
            foreach (KeyValuePair<string, string> kvp in Config.Config.SLOTKEYS)
            {
                slotkeys.Add(kvp.Value);
            }

            if (Player.main.inSeamoth && Player.main.GetPDA().state != PDA.State.Opening)
            {
                newText = slotkeys[index];
                return;
            }
            else if (Player.main.inExosuit && Player.main.GetPDA().state != PDA.State.Opening)
            {
                if (index < 2)
                {
                    newText = "";
                    return;
                }
                else
                {
                    newText = slotkeys[index - 2];
                    return;
                }
            }            
        }        
    }    
}