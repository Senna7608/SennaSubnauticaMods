using UnityEngine;
using Debuggers.UE._TrackedReference;
using Debuggers.UE._Structures;
using Debuggers.UE._Object;

namespace Debuggers.UE._SealedClasses
{
    public static class _AnimationEvent
    {
        public static void DebugAnimationEvent(this AnimationEvent animationEvent, string prefixString)
        {
            animationEvent.animationState.DebugAnimationState($"{prefixString}.animationState");
            animationEvent.animatorClipInfo.DebugAnimatorClipInfo($"{prefixString}.animatorClipInfo");
            animationEvent.animatorStateInfo.DebugAnimatorStateInfo($"{prefixString}.animatorStateInfo");            

            DLog.Log($"{prefixString}.floatParameter: {animationEvent.floatParameter}");
            DLog.Log($"{prefixString}.functionName: {animationEvent.functionName}");
            DLog.Log($"{prefixString}.intParameter: {animationEvent.intParameter}");
            DLog.Log($"{prefixString}.isFiredByAnimator: {animationEvent.isFiredByAnimator}");
            DLog.Log($"{prefixString}.isFiredByLegacy: {animationEvent.isFiredByLegacy}");
            DLog.Log($"{prefixString}.messageOptions: {animationEvent.messageOptions}");

            animationEvent.objectReferenceParameter.DebugUEObject($"{prefixString}.objectReferenceParameter");

            DLog.Log($"{prefixString}.stringParameter: {animationEvent.stringParameter}");
            DLog.Log($"{prefixString}.time: {animationEvent.time}");
        }
    }
}
