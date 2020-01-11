using UnityEngine;
using Debuggers.UE._Object._Motion;

namespace Debuggers.UE._TrackedReference
{
    public static class _AnimationState
    {
        public static void DebugAnimationState(this AnimationState animationState, string prefixString)
        {
            DLog.Log($"{prefixString}.blendMode: {animationState.blendMode}");
            animationState.clip.DebugAnimationClip($"{prefixString}.clip");
            DLog.Log($"{prefixString}.enabled: {animationState.enabled}");
            DLog.Log($"{prefixString}.layer: {animationState.layer}");
            DLog.Log($"{prefixString}.length: {animationState.length}");
            DLog.Log($"{prefixString}.name: {animationState.name}");
            DLog.Log($"{prefixString}.normalizedSpeed: {animationState.normalizedSpeed}");
            DLog.Log($"{prefixString}.normalizedTime: {animationState.normalizedTime}");
            DLog.Log($"{prefixString}.speed: {animationState.speed}");
            DLog.Log($"{prefixString}.time: {animationState.time}");
            DLog.Log($"{prefixString}.weight: {animationState.weight}");
            DLog.Log($"{prefixString}.wrapMode: {animationState.wrapMode}");
        }
    }
}
