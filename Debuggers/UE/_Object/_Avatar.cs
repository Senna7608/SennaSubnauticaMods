using UnityEngine;

namespace Debuggers.UE._Object
{
    public static class _Avatar
    {
        public static void DebugAvatar(this Avatar avatar, string prefixString)
        {
            DLog.Log($"{prefixString}.hideFlags: {avatar.hideFlags}");
            DLog.Log($"{prefixString}.isHuman: {avatar.isHuman}");
            DLog.Log($"{prefixString}.isValid: {avatar.isValid}");
            DLog.Log($"{prefixString}.name: {avatar.name}");
        }
    }
}
