using UnityEngine;

namespace Debuggers.UE._Structures
{
    public static class _UILineInfo
    {
        public static void DebugUILineInfo(this UILineInfo uILineInfo, string prefixString)
        {
            DLog.Log($"{prefixString}.height: {uILineInfo.height}");
            DLog.Log($"{prefixString}.leading: {uILineInfo.leading}");
            DLog.Log($"{prefixString}.startCharIdx: {uILineInfo.startCharIdx}");
            DLog.Log($"{prefixString}.topY: {uILineInfo.topY}");            
        }
    }
}
