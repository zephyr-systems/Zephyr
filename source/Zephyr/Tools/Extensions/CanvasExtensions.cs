using Zephyr.Tools;
using Cosmos.System;
using PrismAPI.Graphics;
using PrismAPI.Hardware.GPU;

namespace Zephyr.GUI
{
    using SVGAIITerminal = SVGAIITerminal.SVGAIITerminal;

    public static unsafe class CanvasExtensions
    {
        private static Display Screen = Kernel.Screen;
        private static SVGAIITerminal Console = Kernel.Console;

        public static void InitializeCursor(this Display _)
        {
            MouseManager.ScreenWidth = Screen.Width;
            MouseManager.ScreenHeight = Screen.Height;
            Logger.Success(Console, "Mouse driver initialized");
        }

        public static void DrawAlphaBlendedImage(this Canvas Canvas, int X, int Y, Canvas Image, bool Alpha = true)
        {
            // Basic null/empty check.
            if (Image == null || Image.Width == 0 || Image.Height == 0)
            {
                return;
            }

            // Quit if nothing needs to be drawn.
            if (X + Image.Width < 0 || Y + Image.Height < 0 || X >= Canvas.Width || Y >= Canvas.Height)
            {
                return;
            }

            // Fast alpha image drawing.
            for (int IY = 0; IY < Image.Height; IY++)
            {
                int CanvasY = Y + IY;

                // Skip if out of bounds.
                if (IY < 0 || IY >= Image.Height || CanvasY < 0 || CanvasY >= Canvas.Height)
                {
                    continue;
                }

                for (int IX = 0; IX < Image.Width; IX++)
                {
                    int CanvasX = X + IX;

                    // Skip if out of bounds.
                    if (IX < 0 || IX >= Image.Width || CanvasX < 0 || CanvasX >= Canvas.Width)
                    {
                        continue;
                    }

                    int CanvasIndex = (CanvasY * Canvas.Width) + CanvasX;

                    // Retrieve background and foreground colors.
                    uint BackgroundARGB = Canvas.Internal[CanvasIndex];
                    uint ForegroundARGB = Image.Internal[(IY * Image.Width) + IX];

                    unchecked
                    {
                        // Extract channels.
                        byte BackgroundR = (byte)((BackgroundARGB >> 16) & 0xFF);
                        byte BackgroundG = (byte)((BackgroundARGB >> 8) & 0xFF);
                        byte BackgroundB = (byte)((BackgroundARGB) & 0xFF);
                        byte ForegroundA = (byte)((ForegroundARGB >> 24) & 0xFF);
                        byte ForegroundR = (byte)((ForegroundARGB >> 16) & 0xFF);
                        byte ForegroundG = (byte)((ForegroundARGB >> 8) & 0xFF);
                        byte ForegroundB = (byte)((ForegroundARGB) & 0xFF);

                        // Inverse the foreground alpha.
                        byte InvForegroundA = (byte)(255 - ForegroundA);

                        // Calculate blending.
                        byte R = (byte)((ForegroundA * ForegroundR + InvForegroundA * BackgroundR) >> 8);
                        byte G = (byte)((ForegroundA * ForegroundG + InvForegroundA * BackgroundG) >> 8);
                        byte B = (byte)((ForegroundA * ForegroundB + InvForegroundA * BackgroundB) >> 8);

                        // Repack channels.
                        uint Color = 0xFF000000 | ((uint)R << 16) | ((uint)G << 8) | B;

                        Canvas.Internal[CanvasIndex] = Color;
                    }
                }
            }
        }

        public static Color AlphaBlend(Color Background, Color Foreground)
        {
            if (Foreground.A == 255)
            {
                return Foreground;
            }
            if (Foreground.A == 0)
            {
                return Background;
            }

            byte alpha = (byte)Foreground.A;
            int invAlpha = (int)(256 - Foreground.A);
            return new()
            {
                A = 255,
                R = (byte)((int)(alpha * Foreground.R + invAlpha * Background.R) >> 8),
                G = (byte)((int)(alpha * Foreground.G + invAlpha * Background.G) >> 8),
                B = (byte)((int)(alpha * Foreground.B + invAlpha * Background.B) >> 8)
            };
        }
    }
}