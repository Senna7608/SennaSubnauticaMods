using UnityEngine;

namespace Debuggers.UE._Object._Component._Behaviour
{
    public static class _Camera
    {
        public static void DebugCamera(this Camera camera, string prefixString = null)
        {
            if (prefixString == null)
            {
                DLog.Log($"\n[Camera] ({camera.name})");
                prefixString = "this";
            }

            DLog.Log($"{prefixString}: *** Not yet Implemented!");
            
                        
        }
    }
}
