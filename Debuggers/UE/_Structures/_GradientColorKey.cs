using UnityEngine;

namespace Debuggers.UE._Structures
{
    public static class _GradientColorKey
    {
        public static void DebugGradientColorKey(this GradientColorKey colorKey, string prefixString)
        {            
            DLog.Log($"{prefixString}.color: {colorKey.color}");
            DLog.Log($"{prefixString}.time: {colorKey.time}");            
        }
    }
}
