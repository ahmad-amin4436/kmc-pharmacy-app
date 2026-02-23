using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ph_App.Utils
{
    public static class LogoHelper
    {
        // Load image and replace near-white background with transparent pixels.
        // This is a simple heuristic useful for single-color backgrounds like white.
        public static Image LoadLogoWithTransparentBackground(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return null;
            try
            {
                using (var src = Image.FromFile(path))
                {
                    var bmp = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
                    using (var g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.Transparent);
                        g.DrawImage(src, 0, 0, src.Width, src.Height);
                    }

                    // Heuristic: treat pixels that are near-white as transparent
                    var threshold = 250; // 0-255, higher = closer to pure white
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            var px = bmp.GetPixel(x, y);
                            if (px.A > 0 && px.R >= threshold && px.G >= threshold && px.B >= threshold)
                            {
                                bmp.SetPixel(x, y, Color.FromArgb(0, px.R, px.G, px.B));
                            }
                        }
                    }

                    return (Image)bmp.Clone();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
