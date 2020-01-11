using UnityEngine.SceneManagement;

namespace Debuggers.UE._Structures
{
    public static class _Scene
    {
        public static void DebugScene(this Scene scene, string prefixString)
        {
            DLog.Log($"{prefixString}.buildIndex: {scene.buildIndex}");
            DLog.Log($"{prefixString}.isDirty: {scene.isDirty}");
            DLog.Log($"{prefixString}.isLoaded: {scene.isLoaded}");
            DLog.Log($"{prefixString}.name: {scene.name}");
            DLog.Log($"{prefixString}.path: {scene.path}");
            DLog.Log($"{prefixString}.rootCount: {scene.rootCount}");
        }
    }
}
