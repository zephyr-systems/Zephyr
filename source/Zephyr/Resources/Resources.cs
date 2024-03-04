using System.IO;
using PrismAPI.Graphics;
using IL2CPU.API.Attribs;
using SVGAIITerminal.TextKit;

namespace Zephyr
{
    public static class Resources
    {
        [ManifestResourceStream(ResourceName = "Zephyr.Resources.Fonts.Plex.acf")] static byte[] _rawPlex;
        [ManifestResourceStream(ResourceName = "Zephyr.Resources.Images.IBeam.bmp")] static byte[] _rawIBeam;
		[ManifestResourceStream(ResourceName = "Zephyr.Resources.Images.Mouse.bmp")] static byte[] _rawMouse;
        [ManifestResourceStream(ResourceName = "Zephyr.Resources.Images.Zephyr.bmp")] static byte[] _rawZephyr;

        public static FontFace Plex = new AcfFontFace(new MemoryStream(_rawPlex));
        public static Canvas IBeam = Image.FromBitmap(_rawIBeam, false);
        public static Canvas Mouse = Image.FromBitmap(_rawMouse, false);
        public static Canvas Zephyr = Image.FromBitmap(_rawZephyr, false);
    }
}