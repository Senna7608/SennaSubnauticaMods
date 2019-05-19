using Harmony;
using Common;
using System.Reflection;
using System;
using UnityEngine;
using QuickSlotExtender.Configuration;

namespace QuickSlotExtender.Patchers
{
    [HarmonyPatch(typeof(QuickSlots))]
    [HarmonyPatch(MethodType.Constructor, new Type[] { typeof(GameObject), typeof(Transform), typeof(Transform), typeof(Inventory), typeof(Transform), typeof(int) })]
    internal class QuickSlots_Constructor_Patch
    {
        static bool isPatched = false;

        static readonly string[] ExpandedQuickSlotNames = new string[13]
        {
            "QuickSlot0",
            "QuickSlot1",
            "QuickSlot2",
            "QuickSlot3",
            "QuickSlot4",
            "QuickSlot5",
            "QuickSlot6",
            "QuickSlot7",
            "QuickSlot8",
            "QuickSlot9",
            "QuickSlot10",
            "QuickSlot11",
            "QuickSlot12",
        };

        [HarmonyPrefix]
        internal static void Prefix(QuickSlots __instance, GameObject owner, Transform toolSocket, Transform cameraSocket, Inventory inv, Transform slotTr, ref int slotCount)
        {
            if (isPatched)
            {
                return;
            }
                        
            __instance.GetType().GetField("slotNames", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField | BindingFlags.SetField).SetValue(__instance, ExpandedQuickSlotNames);
            
            slotCount = QSEConfig.MAXSLOTS;           

            isPatched = true;

            SNLogger.Log($"[{QSEConfig.PROGRAM_NAME}] QuickSlots Constructor patched!");
        }
    }

    
    [HarmonyPatch(typeof(uGUI_QuickSlots))]
    [HarmonyPatch("Init")]
    internal class uGUI_QuickSlots_Init_Patch
    {
        [HarmonyPostfix]
        internal static void Postfix(uGUI_QuickSlots __instance)
        {
            Main.Instance = __instance.gameObject.GetOrAddComponent<QSEHandler>();
            Main.Instance.AddQuickSlotText(__instance);
        }
    }    
}
