using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
#pragma warning disable CS1591

namespace Common
{
    public enum SNLogType
    {
        LOG,
        WARNING,
        ERROR,
        DEBUG,
        TRACE
    }

    public static class SNLogger
    {
        public static readonly Dictionary<SNLogType, string> logTypeCache = new Dictionary<SNLogType, string>(4)
        {
            { SNLogType.LOG, "LOG" },
            { SNLogType.WARNING, "WARNING" },
            { SNLogType.ERROR, "ERROR" },
            { SNLogType.DEBUG, "DEBUG" },
            { SNLogType.TRACE, "TRACE" },
        };            

        private static void WriteLog(SNLogType logType, string message)
        {
            Console.WriteLine($"[{Assembly.GetCallingAssembly().GetName().Name}/{logTypeCache[logType]}] {message}");
        }

        public static void UnityLog(string message) => UnityEngine.Debug.Log(message);

        public static void Log(string message) => WriteLog(SNLogType.LOG, message);

        public static void Log(string format, params object[] args) => WriteLog(SNLogType.LOG, string.Format(format, args));

        public static void Warn(string moduleName, string message) => WriteLog(SNLogType.WARNING, message);

        public static void Warn(string format, params object[] args) => WriteLog(SNLogType.WARNING, string.Format(format, args));

        public static void Error(string message) => WriteLog(SNLogType.ERROR, message);        

        public static void Error(string format, params object[] args) => WriteLog(SNLogType.ERROR, string.Format(format, args));                   

        [Conditional("DEBUG")]
        public static void Debug(string message) => WriteLog(SNLogType.DEBUG, message);

        [Conditional("DEBUG")]
        public static void Debug(string format, params object[] args) => WriteLog(SNLogType.DEBUG, string.Format(format, args));

        [Conditional("TRACE")]
        public static void Trace(string message) => WriteLog(SNLogType.TRACE, message);

        [Conditional("TRACE")]
        public static void Trace(string format, params object[] args) => WriteLog(SNLogType.TRACE, string.Format(format, args));
    }
}
