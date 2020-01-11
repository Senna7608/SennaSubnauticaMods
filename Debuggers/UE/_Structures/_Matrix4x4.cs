using UnityEngine;

namespace Debuggers.UE._Structures
{
    public static class _Matrix4x4
    {
        public static void DebugMatrix4x4(this Matrix4x4 matrix4x4, string prefixString)
        {            
            DLog.Log($"{prefixString}.determinant: {matrix4x4.determinant}");
            DLog.Log($"{prefixString}.isIdentity: {matrix4x4.isIdentity}");
            DLog.Log($"{prefixString}.m00: {matrix4x4.m00}");
            DLog.Log($"{prefixString}.m01: {matrix4x4.m01}");
            DLog.Log($"{prefixString}.m02: {matrix4x4.m02}");
            DLog.Log($"{prefixString}.m03: {matrix4x4.m03}");
            DLog.Log($"{prefixString}.m10: {matrix4x4.m10}");
            DLog.Log($"{prefixString}.m11: {matrix4x4.m11}");
            DLog.Log($"{prefixString}.m12: {matrix4x4.m12}");
            DLog.Log($"{prefixString}.m13: {matrix4x4.m13}");
            DLog.Log($"{prefixString}.m20: {matrix4x4.m20}");
            DLog.Log($"{prefixString}.m21: {matrix4x4.m21}");
            DLog.Log($"{prefixString}.m22: {matrix4x4.m22}");
            DLog.Log($"{prefixString}.m23: {matrix4x4.m23}");
            DLog.Log($"{prefixString}.m30: {matrix4x4.m30}");
            DLog.Log($"{prefixString}.m31: {matrix4x4.m31}");
            DLog.Log($"{prefixString}.m32: {matrix4x4.m32}");
            DLog.Log($"{prefixString}.m33: {matrix4x4.m33}");            
        }
    }
}
