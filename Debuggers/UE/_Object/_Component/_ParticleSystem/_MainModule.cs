using UnityEngine;

namespace Debuggers.UE._Object._Component._ParticleSystem
{
    public static class _MainModule
    {
        public static void DebugMainModule(this ParticleSystem.MainModule main)
        {
            DLog.Log("\n[MainModule]");
            DLog.Log($"customSimulationSpace: {main.customSimulationSpace}");
            DLog.Log($"duration: {main.duration}");
            main.gravityModifier.DebugMinMaxCurve("gravityModifier");
            DLog.Log($"gravityModifierMultiplier: {main.gravityModifierMultiplier}");
            DLog.Log($"loop: {main.loop}");
            DLog.Log($"maxParticles: {main.maxParticles}");
            DLog.Log($"playOnAwake: {main.playOnAwake}");
            DLog.Log($"prewarm: {main.prewarm}");
            DLog.Log($"randomizeRotationDirection: {main.randomizeRotationDirection}");
            DLog.Log($"scalingMode: {main.scalingMode}");
            DLog.Log($"simulationSpace: {main.simulationSpace}");
            DLog.Log($"simulationSpeed: {main.simulationSpeed}");
            main.startColor.DebugMinMaxGradient("startColor");
            main.startDelay.DebugMinMaxCurve("startDelay");
            DLog.Log($"startDelayMultiplier: {main.startDelayMultiplier}");
            main.startLifetime.DebugMinMaxCurve("startLifetime");
            DLog.Log($"startLifetimeMultiplier: {main.startLifetimeMultiplier}");
            main.startRotation.DebugMinMaxCurve("startRotation");
            DLog.Log($"startRotation3D: {main.startRotation3D}");
            DLog.Log($"startRotationMultiplier: {main.startRotationMultiplier}");
            main.startRotationX.DebugMinMaxCurve("startRotationX");
            DLog.Log($"startRotationXMultiplier: {main.startRotationXMultiplier}");
            main.startRotationY.DebugMinMaxCurve("startRotationY");
            DLog.Log($"startRotationXMultiplier: {main.startRotationYMultiplier}");
            main.startRotationZ.DebugMinMaxCurve("startRotationZ");
            DLog.Log($"startRotationZMultiplier: {main.startRotationZMultiplier}");
            main.startSize.DebugMinMaxCurve("startSize");
            DLog.Log($"startSize3D: {main.startSize3D}");
            DLog.Log($"startSizeMultiplier: {main.startSizeMultiplier}");
            main.startSizeX.DebugMinMaxCurve("startSizeX");
            DLog.Log($"startSizeXMultiplier: {main.startSizeXMultiplier}");
            main.startSizeY.DebugMinMaxCurve("startSizeY");
            DLog.Log($"startSizeYMultiplier: {main.startSizeYMultiplier}");
            main.startSizeZ.DebugMinMaxCurve("startSizeZ");
            DLog.Log($"startSizeZMultiplier: {main.startSizeZMultiplier}");
            main.startSpeed.DebugMinMaxCurve("startSpeed");
            DLog.Log($"startSpeedMultiplier: {main.startSpeedMultiplier}");
        }
    }
}
