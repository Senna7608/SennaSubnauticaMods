using UnityEngine;

namespace Debuggers.UE._Object._Component._Behaviour
{
    public static class _Canvas
    {
        public static void DebugCanvas(this Canvas canvas, string prefixString = null)
        {
            if (prefixString == null)
            {
                DLog.Log($"\n[Canvas] ({canvas.name})");
                prefixString = "this";
            }

            DLog.Log($"{prefixString}.additionalShaderChannels: {canvas.additionalShaderChannels}");
            DLog.Log($"{prefixString}.cachedSortingLayerValue: {canvas.cachedSortingLayerValue}");
            DLog.Log($"{prefixString}.enabled: {canvas.enabled}");
            DLog.Log($"{prefixString}.gameObject: {canvas.gameObject.name}");
            DLog.Log($"{prefixString}.hideFlags: {canvas.hideFlags}");
            DLog.Log($"{prefixString}.isActiveAndEnabled: {canvas.isActiveAndEnabled}");
            DLog.Log($"{prefixString}.isRootCanvas: {canvas.isRootCanvas}");
            DLog.Log($"{prefixString}.name: {canvas.name}");
            DLog.Log($"{prefixString}.normalizedSortingGridSize: {canvas.normalizedSortingGridSize}");
            DLog.Log($"{prefixString}.overridePixelPerfect: {canvas.overridePixelPerfect}");
            DLog.Log($"{prefixString}.overrideSorting: {canvas.overrideSorting}");
            DLog.Log($"{prefixString}.pixelPerfect: {canvas.pixelPerfect}");
            DLog.Log($"{prefixString}.pixelRect: {canvas.pixelRect}");
            DLog.Log($"{prefixString}.planeDistance: {canvas.planeDistance}");
            DLog.Log($"{prefixString}.referencePixelsPerUnit: {canvas.referencePixelsPerUnit}");
            DLog.Log($"{prefixString}.renderMode: {canvas.renderMode}");
            DLog.Log($"{prefixString}.renderOrder: {canvas.renderOrder}");
            DLog.Log($"{prefixString}.rootCanvas: {canvas.rootCanvas.name}");
            DLog.Log($"{prefixString}.scaleFactor: {canvas.scaleFactor}");
            DLog.Log($"{prefixString}.sortingLayerID: {canvas.sortingLayerID}");
            DLog.Log($"{prefixString}.sortingLayerName: {canvas.sortingLayerName}");
            DLog.Log($"{prefixString}.sortingOrder: {canvas.sortingOrder}");
            DLog.Log($"{prefixString}.tag: {canvas.tag}");
            DLog.Log($"{prefixString}.targetDisplay: {canvas.targetDisplay}");
            DLog.Log($"{prefixString}.transform: {canvas.transform.name}");
            canvas.worldCamera.DebugCamera($"{prefixString}.worldCamera");            
        }
    }
}
