using System;

namespace Common
{
    public static class SNLogger
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
