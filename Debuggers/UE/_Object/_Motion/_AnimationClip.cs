using UnityEngine;
using Debuggers.UE._SealedClasses;

namespace Debuggers.UE._Object._Motion
{
    public static class _AnimationClip
    {
        public static void DebugAnimationClip(this AnimationClip animationClip, string prefixString)
        {
            DLog.Log($"{prefixString}.apparentSpeed: {animationClip.apparentSpeed}");
            DLog.Log($"{prefixString}.averageAngularSpeed: {animationClip.averageAngularSpeed}");
            DLog.Log($"{prefixString}.averageDuration: {animationClip.averageDuration}");
            DLog.Log($"{prefixString}.averageSpeed: {animationClip.averageSpeed}");

            for (int i = 0; i < animationClip.events.Length; i++)
            {
                animationClip.events[i].DebugAnimationEvent($"{prefixString}.events[{i}]");
            }
        }
    }
}
