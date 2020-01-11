using UnityEngine;
using Debuggers.UE._Object._Component._Behaviour;

namespace Debuggers.UE._Object._Component._ParticleSystem
{
    public static class _LightsModule
    {
        public static void DebugLightsModule(this ParticleSystem.LightsModule lightsModule)
        {
            DLog.Log("\n[LightsModule]");
            DLog.Log($"alphaAffectsIntensity: {lightsModule.alphaAffectsIntensity}");
            DLog.Log($"enabled: {lightsModule.enabled}");
            lightsModule.intensity.DebugMinMaxCurve("intensity");
            DLog.Log($"intensityMultiplier: {lightsModule.intensityMultiplier}");
            lightsModule.light.DebugLight("light");
            DLog.Log($"maxLights: {lightsModule.maxLights}");
            lightsModule.range.DebugMinMaxCurve("range");
            DLog.Log($"rangeMultiplier: {lightsModule.rangeMultiplier}");
            DLog.Log($"ratio: {lightsModule.ratio}");
            DLog.Log($"sizeAffectsRange: {lightsModule.sizeAffectsRange}");
            DLog.Log($"useParticleColor: {lightsModule.useParticleColor}");
            DLog.Log($"useRandomDistribution: {lightsModule.useRandomDistribution}");
        }
    }
}