using System;
using System.IO;
using zephyr.Tools;
using Cosmos.System;

namespace zephyr.CLI.Commands;

using SVGAIITerminal = SVGAIITerminal.SVGAIITerminal;

public static class Unix
{
    public class CD : Script
    {
        public CD() : base("cd", "changes the working directory") { }

        public override void Invoke(SVGAIITerminal Console, string[] Args)
        {
            if (Args.Length < 2)
            {
                Console.WriteLine("Too little arguments!", ConsoleColor.Red);
                return;
            }
            if (Args.Length > 2)
            {
                Console.WriteLine("Too many arguments!", ConsoleColor.Red);
                return;
            }

            switch (Args[1])
            {
                case "..":
                    var currentDir = Directory.GetCurrentDirectory();
                    var newDir = currentDir.Remove(currentDir.LastIndexOf('\\'));

                    if (newDir.Length == 2) newDir += '\\';

                    Directory.SetCurrentDirectory(newDir);
                    break;

                case { } when Args[1].IsFullPath():
                    Directory.SetCurrentDirectory(Args[1]);
                    break;

                default:
                    Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + '\\' + Args[1]);
                    break;
            }
        }
    }

    public class Clear : Script
    {
        public Clear() : base("clear", "clears the terminal") { }

        public override void Invoke(SVGAIITerminal Console, string[] Args)
        {
            if (Args.Length > 1)
            {
                Console.WriteLine("Too many arguments!", ConsoleColor.Red);
                return;
            }

            Console.Clear();
            ConsoleExtensions.ResetTerminalY();
        }
    }

    public class Help : Script
    {
        public Help() : base("help", "displays available commands or gives more info of a command") { }

        public override void Invoke(SVGAIITerminal Console, string[] Args)
        {
            if (Args.Length > 1)
            {
                Console.WriteLine("Too many arguments!", ConsoleColor.Red);
                return;
            }

            Console.WriteLine($"zephyr shell version {Kernel.Version}\n");

            foreach (Script c in Shell.Commands)
            {
                Console.Write(c.Name);
                Console.WriteLine(" - " + c.Description, ConsoleColor.Gray);
            }

            Console.WriteLine("\nFor more info on a specific command, type 'help [command]'");
        }
    }

    public class LS : Script
    {
        public LS() : base("ls", "lists the contents of a directory") { }

        public override void Invoke(SVGAIITerminal Console, string[] Args)
        {
            if (Args.Length > 2)
            {
                Console.WriteLine("Too many arguments!", ConsoleColor.Red);
                return;
            }

            var dir = Args.Length == 2 ? Args[1].IsFullPath() ? Args[1] : Directory.GetCurrentDirectory() + '\\' + Args[1] : Directory.GetCurrentDirectory();

            var files = Directory.GetFiles(dir);
            var directories = Directory.GetDirectories(dir);

            if (files.Length > 0) Console.Write(string.Join("  ", files) + "  ", ConsoleColor.Yellow);
            if (directories.Length > 0) Console.Write(string.Join("  ", directories), ConsoleColor.Blue);

            Console.WriteLine();
        }
    }

    public class PowerOff : Script
    {
        public PowerOff() : base("poweroff", "powers down the system") { }

        public override void Invoke(SVGAIITerminal Console, string[] Args)
        {
            if (Args.Length > 1)
            {
                Console.WriteLine("Too many arguments!", ConsoleColor.Red);
                return;
            }

            Power.Shutdown();
        }
    }

    public class Reboot : Script
    {
        public Reboot() : base("reboot", "reboots the system") { }

        public override void Invoke(SVGAIITerminal Console, string[] Args)
        {
            Power.Reboot();
        }
    }
}