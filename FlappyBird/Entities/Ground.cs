using System.Drawing;
using FlappyBird.Core;

namespace FlappyBird.Entities
{
    /// <summary>
    /// Kayan zemin. Hem kayar (IScrollable) hem de kuş çarparsa oyun biter (ICollidable).
    /// Bir sınıfın birden fazla arayüzü uygulayabileceğine örnektir.
    /// </summary>
    public class Ground : GameObject, ICollidable, IScrollable
    {
        private const float StripeWidth = 24f;
        private const float GrassHeight = 14f;
        private float stripeOffset;

        public Ground() : base(0, GameConfig.PlayAreaBottom, GameConfig.ScreenWidth, GameConfig.GroundHeight)
        {
        }

        public float ScrollSpeed { get; set; }

        public bool CollidesWith(RectangleF area)
        {
            return area.Bottom >= Y;
        }

        public override void Update(float dt)
        {
            stripeOffset = (stripeOffset + ScrollSpeed * dt) % StripeWidth;
        }

        public override void Draw(Graphics g)
        {
            using (var dirt = new SolidBrush(Color.FromArgb(222, 214, 150)))
            using (var grass = new SolidBrush(Color.FromArgb(115, 190, 50)))
            using (var grassDark = new SolidBrush(Color.FromArgb(85, 160, 40)))
            using (var edge = new Pen(Color.FromArgb(60, 90, 30), 3f))
            {
                g.FillRectangle(dirt, X, Y, Width, Height);
                g.FillRectangle(grass, X, Y, Width, GrassHeight);

                // Kayan çapraz şeritler zeminin hareket ettiği hissini verir
                for (float x = -stripeOffset - StripeWidth; x < Width + StripeWidth; x += StripeWidth)
                {
                    PointF[] stripe =
                    {
                        new PointF(x, Y),
                        new PointF(x + StripeWidth / 2, Y),
                        new PointF(x + StripeWidth / 4, Y + GrassHeight),
                        new PointF(x - StripeWidth / 4, Y + GrassHeight)
                    };
                    g.FillPolygon(grassDark, stripe);
                }

                g.DrawLine(edge, X, Y, Width, Y);
                g.DrawLine(edge, X, Y + GrassHeight, Width, Y + GrassHeight);
            }
        }
    }
}
