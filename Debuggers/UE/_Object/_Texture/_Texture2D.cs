using UnityEngine;

namespace Debuggers.UE._Object._Texture
{
    public static class _Texture2D
    {
        public static void DebugTexture2D(this Texture2D texture2D, string prefixString = null)
        {
            if (prefixString == null)
            {
                DLog.Log($"\n[Texture2D] ({texture2D.name})");
                prefixString = "this";
            }

            DLog.Log($"{prefixString}.anisoLevel: {texture2D.anisoLevel}");
            DLog.Log($"{prefixString}.dimension: {texture2D.dimension}");
            DLog.Log($"{prefixString}.filterMode: {texture2D.filterMode}");
            DLog.Log($"{prefixString}.format: {texture2D.format}");
            DLog.Log($"{prefixString}.height: {texture2D.height}");
            DLog.Log($"{prefixString}.hideFlags: {texture2D.hideFlags}");
            DLog.Log($"{prefixString}.mipMapBias: {texture2D.mipMapBias}");
            DLog.Log($"{prefixString}.mipmapCount: {texture2D.mipmapCount}");
            DLog.Log($"{prefixString}.name: {texture2D.name}");
            DLog.Log($"{prefixString}.texelSize: {texture2D.texelSize}");
            DLog.Log($"{prefixString}.width: {texture2D.width}");
            DLog.Log($"{prefixString}.wrapMode: {texture2D.wrapMode}");            
        }
    }
}