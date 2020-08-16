using System;
using UnityEngine;

namespace RuntimeHelper.Logger
{
    public static class RHLogger
    {
        public const string prefixLOG = "[Rh [Log] ";
        public const string prefixWARNING = "[Rh [Warning] ";
        public const string prefixERROR = "[Rh [Error] ";

        public static void RH_Log(string message)
        {
            Console.WriteLine(prefixLOG + message);
        }

        public static void RH_Log(string message, LogType type)
        {
            switch (type)
            {
                case LogType.Assert:
                case LogType.Error:
                case LogType.Exception:
                    RH_Error(message);
                    break;

                case LogType.Warning:
                    RH_Warning(message);
                    break;

                default:
                    RH_Log(message);
                    break;
            }
        }

        public static void RH_Log(string message, LogType type, params object[] args)
        {
            switch (type)
            {
                case LogType.Assert:
                case LogType.Error:
                case LogType.Exception:
                    RH_Error(message, args);
                    break;

                case LogType.Warning:
                    RH_Warning(message, args);
                    break;

                default:
                    RH_Log(message, args);
                    break;
            }
        }

        public static void RH_Log(string format, params object[] args)
        {
            Console.WriteLine(prefixLOG + string.Format(format, args));
        }

        public static void RH_Warning(string message)
        {
            Console.WriteLine(prefixWARNING + message);
        }

        public static void RH_Warning(string format, params object[] args)
        {
            Console.WriteLine(prefixWARNING + string.Format(format, args));
        }

        public static void RH_Error(string message)
        {
            Console.WriteLine(prefixERROR + message);
        }

        public static void RH_Error(string format, params object[] args)
        {
            Console.WriteLine(prefixERROR + string.Format(format, args));
        }
    }
}
