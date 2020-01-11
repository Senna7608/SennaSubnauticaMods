using UnityEngine;

namespace Debuggers.UE._Structures
{
    public static class _KeyFrame
    {
        public static void DebugKeyFrame(this Keyframe keyframe, string prefixString)
        {            
            DLog.Log($"{prefixString}.inTangent: {keyframe.inTangent}");
            DLog.Log($"{prefixString}.outTangent: {keyframe.outTangent}");
            DLog.Log($"{prefixString}.tangentMode: {keyframe.tangentMode}");
            DLog.Log($"{prefixString}.time: {keyframe.time}");
            DLog.Log($"{prefixString}.value: {keyframe.value}");            
        }
    }
}
