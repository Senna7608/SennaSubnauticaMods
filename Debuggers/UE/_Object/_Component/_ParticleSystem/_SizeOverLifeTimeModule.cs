using UnityEngine;

namespace Debuggers.UE._Object._Component._ParticleSystem
{
    public static class _SizeOverLifeTimeModule
    {
        public static void DebugSizeOverLifeTime(this ParticleSystem.SizeOverLifetimeModule sizeOverLifetimeModule)
        {
            DLog.Log("\n[SizeOverLifetimeModule]");
            DLog.Log($"enabled: {sizeOverLifetimeModule.enabled}");
            DLog.Log($"separateAxes: {sizeOverLifetimeModule.separateAxes}");
            sizeOverLifetimeModule.size.DebugMinMaxCurve("size");
            DLog.Log($"sizeMultiplier: {sizeOverLifetimeModule.sizeMultiplier}");
            sizeOverLifetimeModule.x.DebugMinMaxCurve("x");
            DLog.Log($"xMultiplier: {sizeOverLifetimeModule.xMultiplier}");
            sizeOverLifetimeModule.y.DebugMinMaxCurve("y");
            DLog.Log($"yMultiplier: {sizeOverLifetimeModule.yMultiplier}");
            sizeOverLifetimeModule.z.DebugMinMaxCurve("z");
            DLog.Log($"zMultiplier: {sizeOverLifetimeModule.zMultiplier}");
        }
    }
}
