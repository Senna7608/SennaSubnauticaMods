using System;

namespace Debuggers
{
    public static class DLog
    {
        public static void Log(this string message)
        {
            Console.WriteLine(message);
        }

        public static void Log(this string format, params object[] args)
        {
            Console.WriteLine(string.Format(format, args));
        }
    }
}
