using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Ph_App.Forms
{
    /// <summary>
    /// A lightweight responsive base form that attempts to scale controls and fonts
    /// proportionally when the form is resized or maximized.
    /// It is conservative: docked controls keep their docking behavior, while
    /// non-docked controls are resized and moved proportionally.
    /// </summary>
    public class ResponsiveForm : Form
    {
        private Size _designSize;
        private readonly Dictionary<Control, Rectangle> _originalBounds = new Dictionary<Control, Rectangle>();
        private readonly Dictionary<Control, float> _originalFontSizes = new Dictionary<Control, float>();

        public ResponsiveForm()
        {
            this.Load += ResponsiveForm_Load;
            this.Resize += ResponsiveForm_Resize;
            this.AutoScaleMode = AutoScaleMode.None; // we handle scaling manually
        }

        private void ResponsiveForm_Load(object sender, EventArgs e)
        {
            // capture design-time size and control metrics once on load
            try
            {
                _designSize = this.Size;
                _originalBounds.Clear();
                _originalFontSizes.Clear();
                StoreOriginalMetrics(this);
            }
            catch
            {
                // swallow any errors to avoid breaking forms at runtime
            }
        }

        private void ResponsiveForm_Resize(object sender, EventArgs e)
        {
            try
            {
                if (_designSize.Width <= 0 || _designSize.Height <= 0) return;
                var scaleX = (float)this.Width / _designSize.Width;
                var scaleY = (float)this.Height / _designSize.Height;
                ResizeControls(this, scaleX, scaleY);
            }
            catch
            {
                // ignore
            }
        }

        private void StoreOriginalMetrics(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                // record original bounds and font size
                if (!_originalBounds.ContainsKey(c))
                    _originalBounds[c] = c.Bounds;

                if (!_originalFontSizes.ContainsKey(c))
                    _originalFontSizes[c] = c.Font?.Size ?? 8f;

                // recurse
                if (c.HasChildren)
                    StoreOriginalMetrics(c);
            }
        }

        private void ResizeControls(Control parent, float scaleX, float scaleY)
        {
            foreach (Control c in parent.Controls)
            {
                // Do not change controls that are docked; let docking handle them
                if (c.Dock == DockStyle.None)
                {
                    if (_originalBounds.TryGetValue(c, out var rect))
                    {
                        var newX = (int)Math.Round(rect.X * scaleX);
                        var newY = (int)Math.Round(rect.Y * scaleY);
                        var newW = (int)Math.Round(rect.Width * scaleX);
                        var newH = (int)Math.Round(rect.Height * scaleY);
                        c.SetBounds(newX, newY, Math.Max(1, newW), Math.Max(1, newH));
                    }
                }

                // Scale font proportionally (use average scale to be visually balanced)
                if (_originalFontSizes.TryGetValue(c, out var originalFontSize))
                {
                    var avgScale = (scaleX + scaleY) / 2f;
                    var newSize = Math.Max(6f, originalFontSize * avgScale);
                    c.Font = new Font(c.Font.FontFamily, newSize, c.Font.Style);
                }

                // recurse
                if (c.HasChildren)
                    ResizeControls(c, scaleX, scaleY);
            }
        }
    }
}
