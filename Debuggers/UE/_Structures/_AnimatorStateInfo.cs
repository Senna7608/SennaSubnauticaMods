using UnityEngine;

namespace Debuggers.UE._Structures
{
    public static class _AnimatorStateInfo
    {
        public static void DebugAnimatorStateInfo(this AnimatorStateInfo animatorStateInfo, string prefixString)
        {            
            DLog.Log($"{prefixString}.fullPathHash: {animatorStateInfo.fullPathHash}");
            DLog.Log($"{prefixString}.length: {animatorStateInfo.length}");
            DLog.Log($"{prefixString}.loop: {animatorStateInfo.loop}");
            DLog.Log($"{prefixString}.normalizedTime: {animatorStateInfo.normalizedTime}");
            DLog.Log($"{prefixString}.shortNameHash: {animatorStateInfo.shortNameHash}");
            DLog.Log($"{prefixString}.speed: {animatorStateInfo.speed}");
            DLog.Log($"{prefixString}.speedMultiplier: {animatorStateInfo.speedMultiplier}");
            DLog.Log($"{prefixString}.tagHash: {animatorStateInfo.tagHash}");
        }
    }
}
