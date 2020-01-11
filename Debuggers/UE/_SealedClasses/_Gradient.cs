using UnityEngine;
using Debuggers.UE._Structures;

namespace Debuggers.UE._SealedClasses
{
    public static class _Gradient
    {
        public static void DebugGradient(this Gradient gradient, string prefixString)
        {
            for (int i = 0; i < gradient.alphaKeys.Length; i++)
            {
                gradient.alphaKeys[i].DebugGradientAlphaKey($"{prefixString}.alphaKeys[{i}]");                
            }

            for (int i = 0; i < gradient.colorKeys.Length; i++)
            {
                gradient.colorKeys[i].DebugGradientColorKey($"{prefixString}.colorKeys[{i}]");                
            }
                        
            DLog.Log($"{prefixString}.mode: {gradient.mode}");
        }
    }
}
