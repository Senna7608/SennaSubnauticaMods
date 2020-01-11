using UnityEngine;

namespace Debuggers.UE._Structures
{
    public static class _UIVertex
    {
        public static void DebugUIVertex(this UIVertex uIVertex, string prefixString)
        {
            DLog.Log($"{prefixString}.color: {uIVertex.color}");
            DLog.Log($"{prefixString}.normal: {uIVertex.normal}");
            DLog.Log($"{prefixString}.position: {uIVertex.position}");
            DLog.Log($"{prefixString}.tangent: {uIVertex.tangent}");
            DLog.Log($"{prefixString}.uv0: {uIVertex.uv0}");
            DLog.Log($"{prefixString}.uv1: {uIVertex.uv1}");
            DLog.Log($"{prefixString}.uv2: {uIVertex.uv2}");
            DLog.Log($"{prefixString}.uv3: {uIVertex.uv3}");            
        }
    }
}
