using UnityEngine;

namespace Debuggers.UE._Structures
{
    public static class _LayerMask
    {
        public static void DebugLayerMask(this LayerMask layerMask, string prefixString)
        {
            DLog.Log($"{prefixString}.value: {layerMask.value}");
        }        
    }
}
