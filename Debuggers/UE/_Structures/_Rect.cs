using UnityEngine;

namespace Debuggers.UE._Structures
{
    public static class _Rect
    {
        public static void DebugRect(this Rect rect, string prefixString)
        {
            DLog.Log($"{prefixString}.center: {rect.center}");
            DLog.Log($"{prefixString}.height: {rect.height}");
            DLog.Log($"{prefixString}.max: {rect.max}");
            DLog.Log($"{prefixString}.min: {rect.min}");
            DLog.Log($"{prefixString}.position: {rect.position}");
            DLog.Log($"{prefixString}.size: {rect.size}");
            DLog.Log($"{prefixString}.width: {rect.width}");
            DLog.Log($"{prefixString}.x: {rect.x}");
            DLog.Log($"{prefixString}.xMax: {rect.xMax}");
            DLog.Log($"{prefixString}.xMin: {rect.xMin}");
            DLog.Log($"{prefixString}.y: {rect.y}");
            DLog.Log($"{prefixString}.yMax: {rect.yMax}");
            DLog.Log($"{prefixString}.yMin: {rect.yMin}");
        }
    }
}
