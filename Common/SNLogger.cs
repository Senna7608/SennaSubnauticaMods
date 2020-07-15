using System;
using System.Diagnostics;

namespace Common
{
    public static class SNLogger
    {
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }

        public static void Log(string moduleName, string message)
        {
            Console.WriteLine($"[{moduleName}] [LOG] {message}");
        }

        public static void Log(string moduleName, string format, params object[] args)
        {
            Console.WriteLine($"[{moduleName}] [LOG] {string.Format(format, args)}");
        }
        
        public static void Error(string moduleName, string message)
        {
            Console.WriteLine($"[{moduleName}] [ERROR] {message}");
        }

        public static void Error(string moduleName, string format, params object[] args)
        {
            Console.WriteLine($"[{moduleName}] [ERROR] {string.Format(format, args)}");
        }

        public static void Warn(string moduleName, string message)
        {
            Console.WriteLine($"[{moduleName}] [WARNING] {message}");
        }

        public static void Warn(string moduleName, string format, params object[] args)
        {
            Console.WriteLine($"[{moduleName}] [WARNING] {string.Format(format, args)}");
        }

        [Conditional("DEBUG")]
        public static void Debug(string moduleName, string message)
        {
            Console.WriteLine($"[{moduleName}] [DEBUG] {message}");
        }

        [Conditional("DEBUG")]
        public static void Debug(string moduleName, string format, params object[] args)
        {
            Console.WriteLine($"[{moduleName}] [DEBUG] {string.Format(format, args)}");
        }
    }
}
