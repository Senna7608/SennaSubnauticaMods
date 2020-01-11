using UnityEngine;

namespace Debuggers.UE._Object
{
    public static class _Object
    {
        public static void DebugUEObject(this Object uEObject, string prefixString = null)
        {
            DLog.Log($"{prefixString}.hideFlags: {uEObject.hideFlags}");
            DLog.Log($"{prefixString}.name: {uEObject.name}");
        }
    }
}
