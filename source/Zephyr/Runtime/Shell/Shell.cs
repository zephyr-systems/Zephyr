using System;
using PrismAPI.Graphics;

namespace Zephyr.Runtime
{
    using SVGAIITerminal = SVGAIITerminal.SVGAIITerminal;

    public static class Shell
    {
        public static void Main(SVGAIITerminal Console)
        {
            Console.Write(Kernel.Username, ConsoleColor.Yellow);
            Console.Write("@", Color.LightGray);
            Console.Write(Kernel.Hostname, ConsoleColor.Blue);
            Console.Write("> ", Color.LightGray);

            var input = Console.ReadLine().Trim();
            var args = input.Split(' ');
            if (input == string.Empty) return;

            /*foreach (var command in Commands)
            {
                if (command.Name == args[0].ToLower())
                {
                    command.Invoke(Console, args);
                    return;
                }
            }*/

            Console.WriteLine("Invalid command!", ConsoleColor.Red);
        }
    }
}