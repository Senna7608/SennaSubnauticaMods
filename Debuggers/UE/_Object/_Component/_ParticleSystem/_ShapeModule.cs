using UnityEngine;

namespace Debuggers.UE._Object._Component._ParticleSystem
{
    public static class _ShapeModule
    {
        public static void DebugShapeModule(this ParticleSystem.ShapeModule shapeModule)
        {
            DLog.Log("\n[ShapeModule]");
            DLog.Log($"alignToDirection: {shapeModule.alignToDirection}");
            DLog.Log($"angle: {shapeModule.angle}");
            DLog.Log($"arc: {shapeModule.arc}");
            DLog.Log($"arcMode: {shapeModule.arcMode}");

            shapeModule.arcSpeed.DebugMinMaxCurve("arcSpeed");

            DLog.Log($"arcSpeedMultiplier: {shapeModule.arcSpeedMultiplier}");
            DLog.Log($"arcSpread: {shapeModule.arcSpread}");
            DLog.Log($"box: {shapeModule.box}");
            DLog.Log($"enabled: {shapeModule.enabled}");
            DLog.Log($"length: {shapeModule.length}");

            if (shapeModule.mesh == null)
            {
                DLog.Log("mesh: NULL");
            }
            else
            {
                DLog.Log($"mesh.name: {shapeModule.mesh.name}");
            }

            DLog.Log($"meshMaterialIndex: {shapeModule.meshMaterialIndex}");

            if (shapeModule.meshRenderer == null)
            {
                DLog.Log("meshRenderer: NULL");
            }
            else
            {
                DLog.Log($"meshRenderer.name: {shapeModule.meshRenderer.name}");
            }

            DLog.Log($"meshScale: {shapeModule.meshScale}");
            DLog.Log($"meshShapeType: {shapeModule.meshShapeType}");
            DLog.Log($"normalOffset: {shapeModule.normalOffset}");
            DLog.Log($"radius: {shapeModule.radius}");
            DLog.Log($"radiusMode: {shapeModule.radiusMode}");

            shapeModule.radiusSpeed.DebugMinMaxCurve("radiusSpeed");

            DLog.Log($"radiusSpeedMultiplier: {shapeModule.radiusSpeedMultiplier}");
            DLog.Log($"radiusSpread: {shapeModule.radiusSpread}");
            DLog.Log($"randomDirectionAmount: {shapeModule.randomDirectionAmount}");
            DLog.Log($"shapeType: {shapeModule.shapeType}");

            if (shapeModule.skinnedMeshRenderer == null)
            {
                DLog.Log("skinnedMeshRenderer: NULL");
            }
            else
            {
                DLog.Log($"skinnedMeshRenderer.name: {shapeModule.skinnedMeshRenderer.name}");
            }

            DLog.Log($"sphericalDirectionAmount: {shapeModule.sphericalDirectionAmount}");
            DLog.Log($"useMeshColors: {shapeModule.useMeshColors}");
            DLog.Log($"useMeshMaterialIndex: {shapeModule.useMeshMaterialIndex}");
        }
    }
}
