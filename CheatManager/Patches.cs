#if DEBUG
// Only for debugging and testing new functions
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;

namespace CheatManager.Patches
{
    [HarmonyPatch(typeof(Survival))]
    [HarmonyPatch("Eat")]    
    public class Survival_Eat_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                bool flag = instruction.opcode.Equals(OpCodes.Ldc_R4) && instruction.operand.Equals(PatchConstants.baseWaterMax);
                bool flag2 = instruction.opcode.Equals(OpCodes.Ldc_R4) && instruction.operand.Equals(PatchConstants.baseFoodMax);

                if (flag || flag2)
                {
                    yield return new CodeInstruction(OpCodes.Ldc_R4, PatchConstants.overMax)
                    {
                        labels = instruction.labels
                    };
                }

                else
                {
                    yield return instruction;
                }
            }
            
        }
    }
        

    [HarmonyPatch(typeof(Survival))]
    [HarmonyPatch("UpdateStats")]
    public class Survival_UpdateStats_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                bool flag = instruction.opcode.Equals(OpCodes.Ldc_R4) && instruction.operand.Equals(PatchConstants.baseWaterMax);
                bool flag2 = instruction.opcode.Equals(OpCodes.Ldc_R4) && instruction.operand.Equals(PatchConstants.baseFoodMax);

                if (flag || flag2)
                {
                    yield return new CodeInstruction(OpCodes.Ldc_R4, PatchConstants.overMax)
                    {
                        labels = instruction.labels
                    };
                }

                else
                {
                    yield return instruction;
                }
            }
        }
    }


    [HarmonyPatch(typeof(uGUI_FoodBar))]
    [HarmonyPatch("LateUpdate")]
#pragma warning disable IDE1006 // Naming Styles
    public class uGUI_FoodBar_Patch
#pragma warning restore IDE1006 // Naming Styles
    {        
        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, ILGenerator generator, IEnumerable<CodeInstruction> instructions)
        {
            bool injected = false;
            foreach (CodeInstruction instruction in instructions)
            {
                bool flag = instruction.opcode.Equals(OpCodes.Ldc_R4) && !injected;
                if (flag)
                {
                    injected = true;
                    CodeInstruction newInstruction = new CodeInstruction(OpCodes.Ldc_R4, PatchConstants.overMax);
                    newInstruction.labels = instruction.labels;
                    yield return newInstruction;
                    newInstruction = null;
                }
                else
                {
                    yield return instruction;
                }
                
            }            
            yield break;           
        }
    }


    [HarmonyPatch(typeof(uGUI_WaterBar))]
    [HarmonyPatch("LateUpdate")]
#pragma warning disable IDE1006 // Naming Styles
    public class uGUI_WaterBar_Patch
#pragma warning restore IDE1006 // Naming Styles
    {
        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, ILGenerator generator, IEnumerable<CodeInstruction> instructions)
        {
            bool injected = false;
            foreach (CodeInstruction instruction in instructions)
            {
                bool flag = instruction.opcode.Equals(OpCodes.Ldc_R4) && !injected;

                if (flag)
                {
                    injected = true;
                    CodeInstruction newInstruction = new CodeInstruction(OpCodes.Ldc_R4, PatchConstants.overMax)
                    {
                        labels = instruction.labels
                    };
                    yield return newInstruction;
                    newInstruction = null;
                }
                else
                {
                    yield return instruction;
                }
                
            }
            
            yield break;           
        }
    }









}
#endif
