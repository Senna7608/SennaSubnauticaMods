using UnityEngine;
using Debuggers.UE._Object._Motion;

namespace Debuggers.UE._Structures
{
    public static class _AnimatorClipInfo
    {
        public static void DebugAnimatorClipInfo(this AnimatorClipInfo animatorClipInfo, string prefixString)
        {
            animatorClipInfo.clip.DebugAnimationClip($"{prefixString}.clip");
            DLog.Log($"{prefixString}.weight: {animatorClipInfo.weight}");            
        }
    }
}
