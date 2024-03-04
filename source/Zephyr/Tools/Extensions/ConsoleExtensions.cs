using PrismAPI.Graphics;

namespace Zephyr.Tools.Extensions
{
    using SVGAIITerminal = SVGAIITerminal.SVGAIITerminal;

    public static class ConsoleExtensions
    {
        public static void DrawImage(this SVGAIITerminal Console, Canvas Image, bool Alpha = false)
        {
            Console.Contents.DrawImage(0, Console.Font.GetHeight() * Console.CursorTop, Image, Alpha);
            Console.CursorTop += Image.Height / Console.Font.GetHeight();
        }
    }
}