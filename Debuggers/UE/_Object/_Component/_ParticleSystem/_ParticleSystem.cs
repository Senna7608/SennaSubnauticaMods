using UnityEngine;

namespace Debuggers.UE._Object._Component._ParticleSystem
{
    public static class _ParticleSystem
    {
        public static void DebugParticleSystem(this ParticleSystem ps)
        {
            DLog.Log("\n### DebugParticleSystem Started ###");

            DLog.Log($"gameObject: {ps.gameObject.name}");
            DLog.Log($"hideFlags: {ps.hideFlags}");
            DLog.Log($"isEmitting: {ps.isEmitting}");
            DLog.Log($"isPaused: {ps.isPaused}");
            DLog.Log($"isPlaying: {ps.isPlaying}");
            DLog.Log($"isStopped: {ps.isStopped}");
            DLog.Log($"name: {ps.name}");
            DLog.Log($"particleCount: {ps.particleCount}");
            DLog.Log($"randomSeed: {ps.randomSeed}");
            DLog.Log($"tag: {ps.tag}");
            DLog.Log($"time: {ps.time}");
            DLog.Log($"transform: {ps.transform.name}");
            DLog.Log($"useAutoRandomSeed: {ps.useAutoRandomSeed}");

            ps.main.DebugMainModule();

            if (ps.collision.enabled)
            {               
                ps.collision.DebugCollisionModule();
            }

            if (ps.colorBySpeed.enabled)
            {
                DLog.Log("colorBySpeed Module found!");
            }

            if (ps.colorOverLifetime.enabled)
            {
                DLog.Log("ColorOverLifetime Module found!");                
            }

            if (ps.customData.enabled)
            {
                DLog.Log("customData Module found!");
            }

            if (ps.emission.enabled)
            {               
                ps.emission.DebugEmissionModule();
            }

            if (ps.externalForces.enabled)
            {
                DLog.Log("externalForces Module found!");
            }

            if (ps.forceOverLifetime.enabled)
            {
                DLog.Log("forceOverLifetime Module found!");
            }

            if (ps.inheritVelocity.enabled)
            {
                DLog.Log("InheritVelocity Module found!");                
            }

            if (ps.lights.enabled)
            {                
                ps.lights.DebugLightsModule();
            }

            if (ps.limitVelocityOverLifetime.enabled)
            {
                DLog.Log("LimitVelocityOverLifetime Module found!");                
            }            

            if (ps.noise.enabled)
            {
                DLog.Log("noise Module found!");
            }

            if (ps.rotationBySpeed.enabled)
            {
                DLog.Log("rotationBySpeed Module found!");
            }

            if (ps.rotationOverLifetime.enabled)
            {
                DLog.Log("rotationOverLifetime Module found!");
            }

            if (ps.shape.enabled)
            {                
                ps.shape.DebugShapeModule();
            }

            if (ps.sizeBySpeed.enabled)
            {
                DLog.Log("sizeBySpeed Module found!");
            }

            if (ps.sizeOverLifetime.enabled)
            {
                ps.sizeOverLifetime.DebugSizeOverLifeTime();
            }

            if (ps.subEmitters.enabled)
            {
                DLog.Log("subEmitters Module found!");
            }

            if (ps.textureSheetAnimation.enabled)
            {
                DLog.Log("textureSheetAnimation Module found!");
            }

            if (ps.trigger.enabled)
            {                
                ps.trigger.DebugTriggerModule();
            }

            if (ps.velocityOverLifetime.enabled)
            {                
                ps.velocityOverLifetime.DebugVelocityOverLifeTime();
            }

            DLog.Log("\n### DebugParticleSystem Completed ###\n");
        }

        

        




    }
}
