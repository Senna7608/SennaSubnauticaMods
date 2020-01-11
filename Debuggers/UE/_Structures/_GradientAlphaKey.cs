using UnityEngine;

namespace Debuggers.UE._Structures
{
    public static class _GradientAlphaKey
    {
        public static void DebugGradientAlphaKey(this GradientAlphaKey alphaKey, string prefixString)
        {            
            DLog.Log($"{prefixString}.alpha: {alphaKey.alpha}");
            DLog.Log($"{prefixString}.time: {alphaKey.time}");            
        }
    }
}
