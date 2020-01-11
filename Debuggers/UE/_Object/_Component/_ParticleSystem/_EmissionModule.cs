using UnityEngine;

namespace Debuggers.UE._Object._Component._ParticleSystem
{
    public static class _EmissionModule
    {
        public static void DebugEmissionModule(this ParticleSystem.EmissionModule emissionModule)
        {
            DLog.Log("\n[EmissionModule]");
            DLog.Log($"burstCount: {emissionModule.burstCount}");
            DLog.Log($"enabled: {emissionModule.enabled}");
            emissionModule.rateOverDistance.DebugMinMaxCurve("rateOverDistance");
            DLog.Log($"rateOverDistanceMultiplier: {emissionModule.rateOverDistanceMultiplier}");
            emissionModule.rateOverTime.DebugMinMaxCurve("rateOverTime");
            DLog.Log($"rateOverTimeMultiplier: {emissionModule.rateOverTimeMultiplier}");
        }
    }
}
