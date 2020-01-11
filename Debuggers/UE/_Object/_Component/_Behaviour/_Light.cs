using UnityEngine;
using Debuggers.UE._Object._Texture;

namespace Debuggers.UE._Object._Component._Behaviour
{
    public static class _Light
    {
        public static void DebugLight(this Light light, string prefixString = null)
        {
            if (prefixString == null)
            {
                DLog.Log($"\n[Light] ({light.name})");
                prefixString = "this";
            }

            DLog.Log($"{prefixString}.alreadyLightmapped: {light.alreadyLightmapped}");
            DLog.Log($"{prefixString}.bounceIntensity: {light.bounceIntensity}");
            DLog.Log($"{prefixString}.color: {light.color}");
            DLog.Log($"{prefixString}.colorTemperature: {light.colorTemperature}");
            DLog.Log($"{prefixString}.commandBufferCount: {light.commandBufferCount}");

            if (light.cookie != null)
                light.cookie.DebugTexture();

            DLog.Log($"{prefixString}.cookieSize: {light.cookieSize}");
            DLog.Log($"{prefixString}.cullingMask: {light.cullingMask}");
            DLog.Log($"{prefixString}.enabled: {light.enabled}");

            if (light.flare != null)
            {
                DLog.Log($"{prefixString}.flare.name: {light.flare.name}");
                DLog.Log($"{prefixString}.flare.hideFlags: {light.flare.hideFlags}");
            }

            light.gameObject.DebugGameObject($"{prefixString}.gameObject");

            DLog.Log($"{prefixString}.hideFlags: {light.hideFlags}");
            DLog.Log($"{prefixString}.intensity: {light.intensity}");
            DLog.Log($"{prefixString}.isActiveAndEnabled: {light.isActiveAndEnabled}");
            DLog.Log($"{prefixString}.isBaked: {light.isBaked}");
            DLog.Log($"{prefixString}.name: {light.name}");
            DLog.Log($"{prefixString}.range: {light.range}");
            DLog.Log($"{prefixString}.renderMode: {light.renderMode}");
            DLog.Log($"{prefixString}.shadowBias: {light.shadowBias}");
            DLog.Log($"{prefixString}.shadowCustomResolution: {light.shadowCustomResolution}");
            DLog.Log($"{prefixString}.shadowNearPlane: {light.shadowNearPlane}");
            DLog.Log($"{prefixString}.shadowNormalBias: {light.shadowNormalBias}");
            DLog.Log($"{prefixString}.shadowResolution: {light.shadowResolution}");
            DLog.Log($"{prefixString}.shadows: {light.shadows}");
            DLog.Log($"{prefixString}.shadowStrength: {light.shadowStrength}");
            DLog.Log($"{prefixString}.spotAngle: {light.spotAngle}");
            DLog.Log($"{prefixString}.tag: {light.tag}");

            light.transform.DebugTransform($"{prefixString}.transform");

            DLog.Log($"{prefixString}.type: {light.type}");
        }
    }
}
