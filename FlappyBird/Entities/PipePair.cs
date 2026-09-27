using System.Drawing;
using System.Drawing.Drawing2D;
using FlappyBird.Core;

namespace FlappyBird.Entities
{
    /// <summary>
    /// Üst ve alt borudan oluşan klasik engel. Aradaki boşluktan kuş geçer.
    /// </summary>
    public class PipePair : Obstacle
    {
        public const float PipeWidth = 64f;
        private const float CapHeight = 26f;
        private const float CapOverhang = 4f;

        public PipePair(float x, float gapCenterY, float gapSize) : base(x, PipeWidth)
        {
            GapCenterY = gapCenterY;
            GapSize = gapSize;
        }

        /// <summary>Boşluğun merkezi. protected set: alt sınıf (MovingPipePair) değiştirebilir.</summary>
        public float GapCenterY { get; protected set; }
        public float GapSize { get; }

        private float GapTop { get { return GapCenterY - GapSize / 2; } }
        private float GapBottom { get { return GapCenterY + GapSize / 2; } }

        // Borunun renkleri virtual: alt sınıflar farklı renk kullanabilir (polimorfizm).
        protected virtual Color LightColor { get { return Color.FromArgb(140, 215, 80); } }
        protected virtual Color DarkColor { get { return Color.FromArgb(55, 125, 35); } }
        protected virtual Color BorderColor { get { return Color.FromArgb(35, 75, 20); } }

        private RectangleF TopPipe
        {
            get { return new RectangleF(X, 0, Width, GapTop); }
        }

        private RectangleF BottomPipe
        {
            get { return new RectangleF(X, GapBottom, Width, GameConfig.PlayAreaBottom - GapBottom); }
        }

        private RectangleF TopCap
        {
            get { return new RectangleF(X - CapOverhang, GapTop - CapHeight, Width + CapOverhang * 2, CapHeight); }
        }

        private RectangleF BottomCap
        {
            get { return new RectangleF(X - CapOverhang, GapBottom, Width + CapOverhang * 2, CapHeight); }
        }

        public override bool CollidesWith(RectangleF area)
        {
            return area.IntersectsWith(TopPipe)
                || area.IntersectsWith(BottomPipe)
                || area.IntersectsWith(TopCap)
                || area.IntersectsWith(BottomCap);
        }

        public override void Draw(Graphics g)
        {
            DrawSegment(g, TopPipe);
            DrawSegment(g, BottomPipe);
            DrawSegment(g, TopCap);
            DrawSegment(g, BottomCap);
        }

        private void DrawSegment(Graphics g, RectangleF r)
        {
            if (r.Width <= 0 || r.Height <= 0) return;

            // Soldan sağa açık renkten koyu renge geçiş (3B görünüm)
            using (var brush = new LinearGradientBrush(r, LightColor, DarkColor, LinearGradientMode.Horizontal))
            using (var pen = new Pen(BorderColor, 2f))
            {
                g.FillRectangle(brush, r);
                g.DrawRectangle(pen, r.X, r.Y, r.Width, r.Height);
            }
        }
    }
}
