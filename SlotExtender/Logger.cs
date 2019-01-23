using UnityEngine;

namespace SlotExtender
{
    internal static class Logger
    {
        internal static void Log(string message)
        {
            Debug.Log($"[SlotExtender] {message}");
        }

        internal static void Log(string format, params object[] args)
        {
            Debug.Log("[SlotExtender] " + string.Format(format, args));
        }
    }
}
