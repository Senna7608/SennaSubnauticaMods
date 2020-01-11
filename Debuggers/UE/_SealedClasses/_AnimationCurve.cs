using UnityEngine;
using Debuggers.UE._Structures;

namespace Debuggers.UE._SealedClasses
{
    public static class _AnimationCurve
    {
        public static void DebugAnimationCurve(this AnimationCurve animationCurve, string prefixString)
        {
            for (int i = 0; i < animationCurve.keys.Length; i++)
            {
                animationCurve.keys[i].DebugKeyFrame($"{prefixString}.keyframe[{i}]");                
            }
            
            DLog.Log($"{prefixString}.length: {animationCurve.length}");
            DLog.Log($"{prefixString}.postWrapMode: {animationCurve.postWrapMode}");
            DLog.Log($"{prefixString}.preWrapMode: {animationCurve.preWrapMode}");
        }
    }
}
