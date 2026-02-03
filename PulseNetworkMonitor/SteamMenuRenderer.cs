using System.Drawing;
using System.Windows.Forms;

namespace PulseNetworkMonitor
{
    public class SteamMenuRenderer : ToolStripProfessionalRenderer
    {
        private readonly Font _headerFont = new Font("Segoe UI Semibold", 9.5f);
        private readonly Font _itemFont = new Font("Segoe UI Semibold", 9f);
        private const int TextHorizontalPadding = 8;

        public Color PulseColor { get; set; } = Color.LightCyan;

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(28, 28, 28));
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);

            Color bg = e.Item.Selected
                ? Color.FromArgb(55, 55, 55)
                : Color.FromArgb(28, 28, 28);

            using (SolidBrush brush = new SolidBrush(bg))
            {
                e.Graphics.FillRectangle(brush, rect);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            Font font = e.Item.Enabled ? _itemFont : _headerFont;

            Rectangle textRect = e.Item.ContentRectangle;
            textRect.X += TextHorizontalPadding;
            textRect.Width -= TextHorizontalPadding;

            if (!e.Item.Enabled)
            {
                string text = e.Text;
                int pulseIndex = text.IndexOf("Pulse");
                if (pulseIndex >= 0)
                {
                    string before = text.Substring(0, pulseIndex);
                    string pulse = "Pulse";
                    string after = text.Substring(pulseIndex + pulse.Length);

                    TextRenderer.DrawText(e.Graphics, before, font, textRect, Color.FromArgb(180, 180, 180),
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

                    Size beforeSize = TextRenderer.MeasureText(before, font, textRect.Size,
                        TextFormatFlags.Left | TextFormatFlags.NoPadding);

                    Rectangle pulseRect = textRect;
                    pulseRect.X += beforeSize.Width;

                    TextRenderer.DrawText(e.Graphics, pulse, font, pulseRect, PulseColor,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

                    Size pulseSize = TextRenderer.MeasureText(pulse, font, pulseRect.Size,
                        TextFormatFlags.Left | TextFormatFlags.NoPadding);

                    Rectangle afterRect = textRect;
                    afterRect.X += beforeSize.Width + pulseSize.Width;

                    TextRenderer.DrawText(e.Graphics, after, font, afterRect, Color.FromArgb(180, 180, 180),
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

                    return;
                }

                TextRenderer.DrawText(e.Graphics, text, font, textRect, Color.FromArgb(180, 180, 180),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
            else
            {
                TextRenderer.DrawText(e.Graphics, e.Text, font, textRect, Color.Gainsboro,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
        }
    }
}
