using UnityEngine;

namespace SlotExtender
{
    public static class Logger
    {
        public static void Log(string message)
        {
            Debug.Log($"[SlotExtender] {message}");
        }

        public static void Log(string format, params object[] args)
        {
            Debug.Log("[SlotExtender] " + string.Format(format, args));
        }
    }
}
