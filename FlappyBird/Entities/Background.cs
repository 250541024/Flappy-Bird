using System.Drawing;
using System.Drawing.Drawing2D;
using FlappyBird.Core;

namespace FlappyBird.Entities
{
    /// <summary>
    /// Gökyüzü gradyanı ve yavaş kayan tepeler.
    /// </summary>
    public class Background : GameObject, IScrollable
    {
        private const float HillSpacing = 200f;
        private float hillOffset;

        public Background() : base(0, 0, GameConfig.ScreenWidth, GameConfig.PlayAreaBottom)
        {
        }

        public float ScrollSpeed { get; set; }

        public override void Update(float dt)
        {
            hillOffset = (hillOffset + ScrollSpeed * 0.25f * dt) % HillSpacing;
        }

        public override void Draw(Graphics g)
        {
            var skyRect = new RectangleF(X, Y, Width, Height);
            using (var sky = new LinearGradientBrush(skyRect,
                       Color.FromArgb(78, 192, 210), Color.FromArgb(205, 238, 245),
                       LinearGradientMode.Vertical))
            {
                g.FillRectangle(sky, skyRect);
            }

            using (var hill = new SolidBrush(Color.FromArgb(120, 200, 120)))
            using (var hillDark = new SolidBrush(Color.FromArgb(95, 175, 100)))
            {
                float bottom = GameConfig.PlayAreaBottom;
                for (float x = -hillOffset - HillSpacing; x < Width + HillSpacing; x += HillSpacing)
                {
                    g.FillEllipse(hill, x, bottom - 90, 260, 180);
                    g.FillEllipse(hillDark, x + 120, bottom - 55, 180, 110);
                }
            }
        }
    }
}
