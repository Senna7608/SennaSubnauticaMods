using System;
using System.Diagnostics;

namespace RuntimeHelper.Logger
{
    public enum RHLogType
    {
        Error = 0,
        Assert = 1,
        Warning = 2,
        Log = 3,
        Exception = 4,
        Debug = 5
    }

    public static class RHLogger
    {
        public static void Log(string formattedText, RHLogType type)
        {
            if (type == RHLogType.Debug)
            {
                Debug(formattedText);
            }
            else
            {
                Console.WriteLine($"[Rh/{ type}] " + formattedText);
            }            
        }

        public static void Error(string formattedText)
        {
            Log(formattedText, RHLogType.Error);
        }

        public static void Assert(string formattedText)
        {
            Log(formattedText, RHLogType.Assert);
        }

        public static void Warning(string formattedText)
        {
            Log(formattedText, RHLogType.Warning);
        }

        public static void Log(string formattedText)
        {
            Log(formattedText, RHLogType.Log);
        }        

        public static void Exception(string formattedText)
        {
            Log(formattedText, RHLogType.Exception);
        }

        [Conditional("DEBUG")]
        public static void Debug(string formattedText)
        {
            Console.WriteLine($"[Rh/DEBUG] " + formattedText);
        }
    }
}
