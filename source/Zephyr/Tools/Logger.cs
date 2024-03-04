using System;

namespace Zephyr.Tools
{
    using SVGAIITerminal = SVGAIITerminal.SVGAIITerminal;

    public static class Logger
    {
        public static void Success(SVGAIITerminal Console, string Message)
        {
            Console.Write("[  OK  ] ", ConsoleColor.Green);
            Console.WriteLine(Message);
        }

        public static void Warn(SVGAIITerminal Console, string Message)
        {
            Console.Write("[ WARN ] ", ConsoleColor.Yellow);
            Console.WriteLine(Message);
        }

        public static void Fail(string Message)
        {
            if (Kernel.Screen != null) Kernel.Screen.IsEnabled = false;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ FAIL ] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Message);
        }

        public static void Fail(SVGAIITerminal Console, string Message)
        {
            Console.Write("[ FAIL ] ", ConsoleColor.Red);
            Console.WriteLine(Message);
        }
    }
}