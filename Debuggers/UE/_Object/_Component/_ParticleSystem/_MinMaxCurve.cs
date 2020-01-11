using UnityEngine;
using Debuggers.UE._SealedClasses;

namespace Debuggers.UE._Object._Component._ParticleSystem
{
    public static class _MinMaxCurve
    {
        public static void DebugMinMaxCurve(this ParticleSystem.MinMaxCurve minMaxCurve, string prefixString)
        {
            DLog.Log($"{prefixString}.constant: {minMaxCurve.constant}");
            DLog.Log($"{prefixString}.constantMax: {minMaxCurve.constantMax}");
            DLog.Log($"{prefixString}.constantMin: {minMaxCurve.constantMin}");

            if (minMaxCurve.curve == null)
            {
                DLog.Log($"{prefixString}.curve: NULL");
            }
            else
            {
                minMaxCurve.curve.DebugAnimationCurve($"{prefixString}.curve");
            }

            if (minMaxCurve.curveMax == null)
            {
                DLog.Log($"{prefixString}.curveMax: NULL");
            }
            else
            {
                minMaxCurve.curveMax.DebugAnimationCurve($"{prefixString}.curveMax");
            }

            if (minMaxCurve.curveMin == null)
            {
                DLog.Log($"{prefixString}.curveMin: NULL");
            }
            else
            {
                minMaxCurve.curveMin.DebugAnimationCurve($"{prefixString}.curveMin");
            }

            DLog.Log($"{prefixString}.curveMultiplier: {minMaxCurve.curveMultiplier}");
            DLog.Log($"{prefixString}.mode: {minMaxCurve.mode}");
        }

    }
}
