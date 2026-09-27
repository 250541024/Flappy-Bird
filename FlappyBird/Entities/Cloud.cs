using System;
using System.Drawing;
using FlappyBird.Core;

namespace FlappyBird.Entities
{
    /// <summary>
    /// Arka planda yavaşça kayan bulut. Ekrandan çıkınca silinmek yerine
    /// sağdan yeniden doğar (OnLeftScreen override edilir).
    /// </summary>
    public class Cloud : ScrollingObject
    {
        private static readonly Random random = new Random();
        private readonly float scale;

        private Cloud(float x, float y, float scale)
            : base(x, y, 110 * scale, 45 * scale, 0.15f * scale)
        {
            this.scale = scale;
        }

        /// <summary>Fabrika metodu: rastgele boyut ve konumda bir bulut oluşturur.</summary>
        public static Cloud CreateRandom()
        {
            float scale = 0.6f + (float)random.NextDouble() * 0.8f;
            return new Cloud(random.Next(0, GameConfig.ScreenWidth), random.Next(20, 240), scale);
        }

        protected override void OnLeftScreen()
        {
            X = GameConfig.ScreenWidth + random.Next(20, 150);
            Y = random.Next(20, 240);
        }

        public override void Draw(Graphics g)
        {
            float s = scale;
            using (var brush = new SolidBrush(Color.FromArgb(230, 255, 255, 255)))
            {
                g.FillEllipse(brush, X, Y + 15 * s, 60 * s, 30 * s);
                g.FillEllipse(brush, X + 25 * s, Y, 50 * s, 40 * s);
                g.FillEllipse(brush, X + 55 * s, Y + 15 * s, 50 * s, 28 * s);
            }
        }
    }
}
