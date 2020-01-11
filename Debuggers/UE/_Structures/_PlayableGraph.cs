using UnityEngine.Experimental.Director;

namespace Debuggers.UE._Structures
{
    public static class _PlayableGraph
    {
        public static void DebugPlayableGraph(this PlayableGraph playableGraph, string prefixString)
        {
            DLog.Log($"{prefixString}.isDone: {playableGraph.isDone}");
            DLog.Log($"{prefixString}.playableCount: {playableGraph.playableCount}");
            DLog.Log($"{prefixString}.rootPlayableCount: {playableGraph.rootPlayableCount}");
            DLog.Log($"{prefixString}.scriptOutputCount: {playableGraph.scriptOutputCount}");           
        }
    }
}
