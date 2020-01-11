using System;
using UnityEngine;

namespace RuntimeHelper.Logger
{
    public static class RHLogger
    {
        public static void RH_Log(this string message)
        {
            Console.WriteLine("[RuntimeHelper] " + message);
        }

        public static void RH_Log(this string message, LogType type)
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

        public static void RH_Log(this string message, LogType type, params object[] args)
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

        public static void RH_Log(this string format, params object[] args)
        {
            Console.WriteLine("[RuntimeHelper] " + string.Format(format, args));
        }

        public static void RH_Warning(this string message)
        {
            Console.WriteLine("[RuntimeHelper] WARNING! " + message);
        }

        public static void RH_Warning(this string format, params object[] args)
        {
            Console.WriteLine("[RuntimeHelper] WARNING! " + string.Format(format, args));
        }

        public static void RH_Error(this string message)
        {
            Console.WriteLine("[RuntimeHelper] *** ERROR " + message);
        }

        public static void RH_Error(this string format, params object[] args)
        {
            Console.WriteLine("[RuntimeHelper] *** ERROR " + string.Format(format, args));
        }
    }
}
