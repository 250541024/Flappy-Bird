using System;
using System.Drawing;

namespace FlappyBird.Entities
{
    /// <summary>
    /// Boşluğu yukarı-aşağı hareket eden boru. İleri seviyelerde ortaya çıkar.
    /// PipePair'den KALITIM alır; sadece farklı olan davranışı (Update) ve
    /// renkleri OVERRIDE eder, geri kalan her şeyi (çizim, çarpışma) üst sınıftan kullanır.
    /// </summary>
    public class MovingPipePair : PipePair
    {
        private readonly float baseCenterY;
        private readonly float amplitude;
        private float time;

        public MovingPipePair(float x, float gapCenterY, float gapSize, float amplitude)
            : base(x, gapCenterY, gapSize)
        {
            baseCenterY = gapCenterY;
            this.amplitude = amplitude;
        }

        public override void Update(float dt)
        {
            base.Update(dt); // Önce normal kayma hareketi (üst sınıfın davranışı)

            time += dt;
            GapCenterY = baseCenterY + (float)Math.Sin(time * 0.05f) * amplitude;
        }

        protected override Color LightColor { get { return Color.FromArgb(240, 150, 80); } }
        protected override Color DarkColor { get { return Color.FromArgb(170, 70, 30); } }
        protected override Color BorderColor { get { return Color.FromArgb(100, 40, 15); } }
    }
}
