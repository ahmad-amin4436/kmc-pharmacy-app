using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Ph_App.Utils
{
    public static class LogoHelper
    {
        // Load image and replace near-white background with transparent pixels.
        // Enhanced with high-quality rendering to prevent blur
        public static Image LoadLogoWithTransparentBackground(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return null;
            try
            {
                using (var src = Image.FromFile(path))
                {
                    // Create high-quality bitmap with original dimensions
                    var bmp = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
                    
                    using (var g = Graphics.FromImage(bmp))
                    {
                        // Set highest quality rendering settings
                        g.Clear(Color.Transparent);
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        
                        // Draw at original size to maintain quality
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

        // Load high-quality logo with proper scaling for PictureBox
        public static Image LoadLogoForPictureBox(string path, Size targetSize)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return null;
            try
            {
                using (var src = Image.FromFile(path))
                {
                    // Calculate scaling to maintain aspect ratio
                    var scale = Math.Min((double)targetSize.Width / src.Width, (double)targetSize.Height / src.Height);
                    var scaledWidth = (int)(src.Width * scale);
                    var scaledHeight = (int)(src.Height * scale);
                    
                    var bmp = new Bitmap(scaledWidth, scaledHeight, PixelFormat.Format32bppArgb);
                    
                    using (var g = Graphics.FromImage(bmp))
                    {
                        // Set highest quality rendering
                        g.Clear(Color.Transparent);
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        
                        g.DrawImage(src, 0, 0, scaledWidth, scaledHeight);
                    }

                    // Remove white background
                    var threshold = 250;
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
