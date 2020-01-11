using UnityEngine;

namespace Debuggers.UE._Object._Component._ParticleSystem
{
    public static class _VelocityOverLifeTimeModule
    {
        public static void DebugVelocityOverLifeTime(this ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule)
        {
            DLog.Log("\n[VelocityOverLifetimeModule]");
            DLog.Log($"enabled: {velocityOverLifetimeModule.enabled}");
            DLog.Log($"space: {velocityOverLifetimeModule.space}");
            velocityOverLifetimeModule.x.DebugMinMaxCurve("x");
            DLog.Log($"xMultiplier: {velocityOverLifetimeModule.xMultiplier}");
            velocityOverLifetimeModule.y.DebugMinMaxCurve("y");
            DLog.Log($"xMultiplier: {velocityOverLifetimeModule.yMultiplier}");
            velocityOverLifetimeModule.z.DebugMinMaxCurve("z");
            DLog.Log($"xMultiplier: {velocityOverLifetimeModule.zMultiplier}");
        }
    }
}
