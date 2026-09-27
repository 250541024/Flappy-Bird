using System.Drawing;
using System.Drawing.Drawing2D;

namespace FlappyBird.Rendering
{
    /// <summary>
    /// Arayüz çizimleri için ortak yardımcı metotlar (static sınıf).
    /// </summary>
    public static class DrawHelper
    {
        public static readonly Font SmallFont = new Font("Arial", 10f, FontStyle.Bold);
        public static readonly Color OutlineColor = Color.FromArgb(70, 45, 20);
        public static readonly Color PanelColor = Color.FromArgb(245, 240, 215);
        public static readonly Color PanelBorderColor = Color.FromArgb(120, 90, 50);

        /// <summary>Kenarlıklı, ortalanmış, kalın yazı çizer.</summary>
        public static void DrawOutlinedText(Graphics g, string text, float size, float centerX, float y,
                                            Color fill, float outlineWidth = 5f)
        {
            using (var path = new GraphicsPath())
            using (var family = new FontFamily("Arial"))
            using (var format = new StringFormat { Alignment = StringAlignment.Center })
            {
                path.AddString(text, family, (int)FontStyle.Bold, size, new PointF(centerX, y), format);

                using (var pen = new Pen(OutlineColor, outlineWidth) { LineJoin = LineJoin.Round })
                {
                    g.DrawPath(pen, path);
                }
                using (var brush = new SolidBrush(fill))
                {
                    g.FillPath(brush, path);
                }
            }
        }

        /// <summary>Küçük, ortalanmış düz yazı çizer.</summary>
        public static void DrawSmallText(Graphics g, string text, float centerX, float y, Color color)
        {
            using (var brush = new SolidBrush(color))
            using (var format = new StringFormat { Alignment = StringAlignment.Center })
            {
                g.DrawString(text, SmallFont, brush, centerX, y, format);
            }
        }

        public static void FillRoundedRect(Graphics g, RectangleF r, float radius, Color fill, Color border)
        {
            float d = radius * 2;
            using (var path = new GraphicsPath())
            {
                path.AddArc(r.X, r.Y, d, d, 180, 90);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                path.CloseFigure();

                using (var brush = new SolidBrush(fill))
                using (var pen = new Pen(border, 3f))
                {
                    g.FillPath(brush, path);
                    g.DrawPath(pen, path);
                }
            }
        }

        /// <summary>Tüm ekranı yarı saydam bir renkle kaplar.</summary>
        public static void FillScreen(Graphics g, Color color, int width, int height)
        {
            using (var brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, 0, 0, width, height);
            }
        }
    }
}
