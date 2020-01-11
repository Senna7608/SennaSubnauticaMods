using UnityEngine;

namespace Debuggers.UE._SealedClasses
{
    public static class _AnimatorControllerParameter
    {
        public static void DebugAnimatorControllerParameter(this AnimatorControllerParameter animatorControllerParameter, string prefixString)
        {
            DLog.Log($"{prefixString}.defaultBool: {animatorControllerParameter.defaultBool}");
            DLog.Log($"{prefixString}.defaultFloat: {animatorControllerParameter.defaultFloat}");
            DLog.Log($"{prefixString}.defaultInt: {animatorControllerParameter.defaultInt}");
            DLog.Log($"{prefixString}.name: {animatorControllerParameter.name}");
            DLog.Log($"{prefixString}.nameHash: {animatorControllerParameter.nameHash}");
            DLog.Log($"{prefixString}.type: {animatorControllerParameter.type}");
        }
    }
}
