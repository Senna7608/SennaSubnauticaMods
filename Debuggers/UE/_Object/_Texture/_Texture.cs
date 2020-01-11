using UnityEngine;

namespace Debuggers.UE._Object._Texture
{
    public static class _Texture
    {
        public static void DebugTexture(this Texture texture, string prefixString = null)
        {          
            if (prefixString == null)
            {
                DLog.Log($"\n[Texture] ({texture.name})");
                prefixString = "this";
            }

            DLog.Log($"{prefixString}.anisoLevel: {texture.anisoLevel}");
            DLog.Log($"{prefixString}.dimension: {texture.dimension}");
            DLog.Log($"{prefixString}.filterMode: {texture.filterMode}");
            DLog.Log($"{prefixString}.height: {texture.height}");
            DLog.Log($"{prefixString}.hideFlags: {texture.hideFlags}");
            DLog.Log($"{prefixString}.mipMapBias: {texture.mipMapBias}");
            DLog.Log($"{prefixString}.name: {texture.name}");
            DLog.Log($"{prefixString}.texelSize: {texture.texelSize}");
            DLog.Log($"{prefixString}.width: {texture.width}");
            DLog.Log($"{prefixString}.wrapMode: {texture.wrapMode}");
        }
    }
}
