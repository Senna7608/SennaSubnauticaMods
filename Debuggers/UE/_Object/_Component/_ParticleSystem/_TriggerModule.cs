using UnityEngine;

namespace Debuggers.UE._Object._Component._ParticleSystem
{
    public static class _TriggerModule
    {
        public static void DebugTriggerModule(this ParticleSystem.TriggerModule triggerModule)
        {
            DLog.Log("\n[TriggerModule]");
            DLog.Log($"enabled: {triggerModule.enabled}");
            DLog.Log($"enter: {triggerModule.enter}");
            DLog.Log($"exit: {triggerModule.exit}");
            DLog.Log($"inside: {triggerModule.inside}");
            DLog.Log($"maxColliderCount: {triggerModule.maxColliderCount}");
            DLog.Log($"outside: {triggerModule.outside}");
            DLog.Log($"radiusScale: {triggerModule.radiusScale}");
        }
    }
}
