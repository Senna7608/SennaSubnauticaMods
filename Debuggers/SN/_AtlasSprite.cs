using Debuggers.UE._Object._Texture;

namespace Debuggers.SN
{
    public static class _AtlasSprite
    {
        public static void DebugAtlasSprite(this Atlas.Sprite sprite, string prefixString = null)
        {
            if (prefixString == null)
            {
                DLog.Log($"\n[Sprite]");
                prefixString = "this";
            }
            
            DLog.Log($"{prefixString}.border: {sprite.border}");
            DLog.Log($"{prefixString}.inner: {sprite.inner}");
            DLog.Log($"{prefixString}.outer: {sprite.outer}");
            DLog.Log($"{prefixString}.padding: {sprite.padding}");
            DLog.Log($"{prefixString}.pixelsPerUnit: {sprite.pixelsPerUnit}");
            DLog.Log($"{prefixString}.size: {sprite.size}");
            DLog.Log($"{prefixString}.slice9Grid: {sprite.slice9Grid}");

            sprite.texture.DebugTexture2D($"{prefixString}.texture");

            for (int i = 0; i < sprite.triangles.Length; i++)
            {                
                DLog.Log($"{prefixString}.triangles[{i}]: {sprite.triangles[i]}");
            }

            for (int i = 0; i < sprite.uv0.Length; i++)
            {
                DLog.Log($"{prefixString}.uv0[{i}]: {sprite.uv0[i]}");
            }

            for (int i = 0; i < sprite.vertices.Length; i++)
            {
                DLog.Log($"{prefixString}.vertices[{i}]: {sprite.vertices[i]}");
            }
        }
    }
}
