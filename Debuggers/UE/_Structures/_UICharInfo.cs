using UnityEngine;

namespace Debuggers.UE._Structures
{
    public static class _UICharInfo
    {
        public static void DebugUICharInfo(this UICharInfo uICharInfo, string prefixString)
        {
            DLog.Log($"{prefixString}.height: {uICharInfo.charWidth}");
            DLog.Log($"{prefixString}.leading: {uICharInfo.cursorPos}");                        
        }
    }
}
