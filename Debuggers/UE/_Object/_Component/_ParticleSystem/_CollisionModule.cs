using UnityEngine;
using Debuggers.UE._Structures;

namespace Debuggers.UE._Object._Component._ParticleSystem
{
    public static class _CollisionModule
    {
        public static void DebugCollisionModule(this ParticleSystem.CollisionModule collisionModule)
        {
            DLog.Log("\n[CollisionModule]");
            collisionModule.bounce.DebugMinMaxCurve("bounce");
            DLog.Log($"bounceMultiplier: {collisionModule.bounceMultiplier}");
            collisionModule.collidesWith.DebugLayerMask("collidesWith");
            collisionModule.dampen.DebugMinMaxCurve("dampen");
            DLog.Log($"dampenMultiplier: {collisionModule.dampenMultiplier}");
            DLog.Log($"enabled: {collisionModule.enabled}");
            DLog.Log($"enableDynamicColliders: {collisionModule.enableDynamicColliders}");
            DLog.Log($"enableInteriorCollisions: {collisionModule.enableInteriorCollisions}");
            collisionModule.lifetimeLoss.DebugMinMaxCurve("lifetimeLoss");
            DLog.Log($"lifetimeLossMultiplier: {collisionModule.lifetimeLossMultiplier}");
            DLog.Log($"maxCollisionShapes: {collisionModule.maxCollisionShapes}");
            DLog.Log($"maxKillSpeed: {collisionModule.maxKillSpeed}");
            DLog.Log($"maxPlaneCount: {collisionModule.maxPlaneCount}");
            DLog.Log($"minKillSpeed: {collisionModule.minKillSpeed}");
            DLog.Log($"mode: {collisionModule.mode}");
            DLog.Log($"quality: {collisionModule.quality}");
            DLog.Log($"radiusScale: {collisionModule.radiusScale}");
            DLog.Log($"sendCollisionMessages: {collisionModule.sendCollisionMessages}");
            DLog.Log($"type: {collisionModule.type}");
            DLog.Log($"voxelSize: {collisionModule.voxelSize}");
        }
    }
}
