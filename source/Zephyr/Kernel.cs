using System;
using Zephyr.Tools;
using Zephyr.Runtime;
using PrismAPI.Graphics;
using Cosmos.Core.Memory;
using PrismAPI.Hardware.GPU;
using Zephyr.Tools.Extensions;
using Zephyr.GUI;
using Cosmos.System;

namespace Zephyr
{
    using SVGAIITerminal = SVGAIITerminal.SVGAIITerminal;

    public class Kernel : Cosmos.System.Kernel
    {
        public const string Version = "0.1";

        public static string Username = "root";
        public static string Hostname = "zephyr";

        public static Display Screen;
        public static SVGAIITerminal Console;

        protected override void BeforeRun()
        {
            System.Console.Clear();

            try
            {
                Screen = Display.GetDisplay(1024, 768);
                Console = new SVGAIITerminal((ushort)(Screen.Width - TerminalHelper.ScrollBarWidth),
                    Screen.Height * 3, Resources.Plex, TerminalHelper.Update)
                {
                    IdleRequest = TerminalHelper.Idle,
                    ScrollRequest = TerminalHelper.Scroll,
                    ParentHeight = Screen.Height / Resources.Plex.GetHeight()
                };

                Logger.Success(Console, "Display driver initialized");
                Logger.Success(Console, "Framebuffer terminal initialized");

                Screen.InitializeCursor();

                Console.Clear();

                Console.Write("Welcome to ");
                Console.Write("Zephyr", ConsoleColor.Cyan);
                Console.WriteLine("!");

                Console.DrawImage(Resources.Zephyr);

                Console.WriteLine("\n * GitHub repository: https://github.com/zephyr-systems/Zephyr\n", ConsoleColor.Green);
            }
            catch (NotImplementedException ex)
            {
                CrashHelpers.PrintGenericErrorMessage(ex.Message == "No display is available!" ?
                    "Unsupported graphics card!\nPlease install a SVGAII or VBE compatible graphics card" :
                    "An exception happened that didn't get handled\nException: " + ex.Message);
            }
            catch (Exception ex)
            {
                CrashHelpers.PrintGenericErrorMessage(
                    "An exception happened that didn't get handled\nException: " + ex.Message);
            }
        }

        protected override void Run() => Shell.Main(Console);

        private static class TerminalHelper
        {
            private static int TerminalY = 0;
            private static int FramesToHeapCollect = 15;

            internal const int ScrollBarWidth = 6;

            internal static void Update() => Update(true);

            private static void Update(bool HeapCollect)
            {
                Screen.Clear(Console.BackgroundColor);
                Screen.DrawImage(0, TerminalY, Console.Contents, false);
                Screen.DrawFilledRectangle(Screen.Width - ScrollBarWidth, -TerminalY / 3,
                    ScrollBarWidth, (ushort)(Screen.Height / 3), 0, new Color(192, 192, 192));
                Screen.DrawAlphaBlendedImage((int)MouseManager.X, (int)MouseManager.Y, Resources.Mouse);
                Screen.Update();

                if (HeapCollect) Heap.Collect();
            }

            internal static void Idle()
            {
                if (MouseManager.ScrollDelta != 0)
                {
                    TerminalY -= MouseManager.ScrollDelta * Console.Font.GetHeight() * 3;

                    switch (TerminalY)
                    {
                        case { } when TerminalY > 0:
                            TerminalY = 0;
                            break;

                        case { } when TerminalY < -(Console.CursorTop * Console.Font.GetHeight()):
                            TerminalY = -(Console.CursorTop * Console.Font.GetHeight());
                            break;

                        case { } when TerminalY < -(Console.Contents.Height - Screen.Height):
                            TerminalY = -(Console.Contents.Height - Screen.Height);
                            break;
                    }

                    MouseManager.ResetScrollDelta();
                }

                Update(false);

                if (FramesToHeapCollect <= 0)
                {
                    Heap.Collect();
                    FramesToHeapCollect = 15;
                }
            }

            internal static void Scroll()
                => TerminalY = Screen.Height - ((Console.CursorTop + 1) * Console.Font.GetHeight());
        }

        private static class CrashHelpers
        {
            internal static void PrintGenericErrorMessage(string Message)
            {
                foreach (string line in Message.Split('\n')) Logger.Fail(line);

                System.Console.WriteLine("Press any key to reboot...");
                System.Console.ReadKey(true);

                Power.Reboot();
            }
        }
    }
}