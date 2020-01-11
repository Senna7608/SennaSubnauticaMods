using UnityEngine;
using Debuggers.UE._Object._Motion;

namespace Debuggers.UE._Object
{
    public static class _RuntimeAnimatorController
    {
        public static void DebugRuntimeAnimatorController(this RuntimeAnimatorController runtimeAnimatorController, string prefixString)
        {
            for (int i = 0; i < runtimeAnimatorController.animationClips.Length; i++)
            {
                runtimeAnimatorController.animationClips[i].DebugAnimationClip($"{prefixString}.animationClips[{i}]");
            }
        }
    }
}
