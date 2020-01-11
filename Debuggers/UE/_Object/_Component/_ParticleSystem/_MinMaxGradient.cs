using UnityEngine;
using Debuggers.UE._SealedClasses;

namespace Debuggers.UE._Object._Component._ParticleSystem
{
    public static class _MinMaxGradient
    {
        public static void DebugMinMaxGradient(this ParticleSystem.MinMaxGradient minMaxGradient, string prefixString)
        {
            DLog.Log($"{prefixString}.color: {minMaxGradient.color}");
            DLog.Log($"{prefixString}.colorMax: {minMaxGradient.colorMax}");
            DLog.Log($"{prefixString}.colorMin: {minMaxGradient.colorMin}");
                                   
            if (minMaxGradient.gradient == null)
            {
                DLog.Log($"{prefixString}.gradient: NULL");
            }
            else
            {
                minMaxGradient.gradient.DebugGradient($"{prefixString}.gradient");
            }

            if (minMaxGradient.gradientMax == null)
            {
                DLog.Log($"{prefixString}.gradientMax: NULL");
            }
            else
            {
                minMaxGradient.gradientMax.DebugGradient($"{prefixString}.gradientMax");
            }

            if (minMaxGradient.gradientMin == null)
            {
                DLog.Log($"{prefixString}.gradientMin: NULL");
            }
            else
            {
                minMaxGradient.gradientMin.DebugGradient($"{prefixString}.gradientMin");
            }

            DLog.Log($"{prefixString}.mode: {minMaxGradient.mode}");
        }
    }
}
