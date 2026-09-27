using System;
using System.Drawing;
using FlappyBird.Core;

namespace FlappyBird.Entities
{
    /// <summary>
    /// Oyuncunun kontrol ettiği kuş. GameObject'ten KALITIM alır.
    /// </summary>
    public class Bird : GameObject
    {
        private const float BirdWidth = 34f;
        private const float BirdHeight = 24f;

        private readonly float startY;   // Menüde süzülürken referans yükseklik
        private float rotation;          // Derece cinsinden açı
        private float wingTimer;         // Kanat animasyonu sayacı

        public Bird(float x, float y) : base(x, y, BirdWidth, BirdHeight)
        {
            startY = y;
        }

        public float Velocity { get; private set; }
        public bool IsDead { get; private set; }

        /// <summary>
        /// OVERRIDE: Çarpışma kutusu görselden biraz küçüktür, böylece oyun daha adil hissettirir.
        /// </summary>
        public override RectangleF Bounds
        {
            get { return new RectangleF(X + 4, Y + 3, Width - 8, Height - 6); }
        }

        public void Flap()
        {
            if (!IsDead) Velocity = GameConfig.FlapStrength;
        }

        public void Kill()
        {
            IsDead = true;
            if (Velocity < 0) Velocity = 0;
        }

        public bool IsOnGround(float groundTop)
        {
            return Y + Height >= groundTop;
        }

        public void LandOn(float groundTop)
        {
            Y = groundTop - Height;
            Velocity = 0;
        }

        /// <summary>Menü ekranında kuşun yukarı aşağı süzülmesi.</summary>
        public void Hover(float time, float dt)
        {
            Y = startY + (float)Math.Sin(time * 0.08f) * 8f;
            rotation = 0f;
            wingTimer += dt;
        }

        public override void Update(float dt)
        {
            // Yerçekimi
            Velocity = Math.Min(Velocity + GameConfig.Gravity * dt, GameConfig.MaxFallSpeed);
            Y += Velocity * dt;

            // Tavan: kuş ekranın üstünden çıkamaz
            if (!IsDead && Y < 0)
            {
                Y = 0;
                Velocity = 0;
            }

            // Yükselirken yukarı, düşerken aşağı baksın (yumuşak geçişle)
            float targetRotation = IsDead ? 90f : Math.Clamp(Velocity * 7f, -25f, 90f);
            rotation += (targetRotation - rotation) * Math.Min(1f, 0.2f * dt);

            if (!IsDead) wingTimer += dt;
        }

        public override void Draw(Graphics g)
        {
            var saved = g.Save();
            g.TranslateTransform(X + Width / 2, Y + Height / 2); // Kuşun merkezine taşı
            g.RotateTransform(rotation);

            float w = Width, h = Height;

            using (var outline = new Pen(Color.FromArgb(70, 45, 20), 2f))
            using (var bodyBrush = new SolidBrush(Color.FromArgb(250, 205, 45)))
            using (var bellyBrush = new SolidBrush(Color.FromArgb(255, 240, 170)))
            using (var wingBrush = new SolidBrush(Color.FromArgb(255, 250, 225)))
            using (var eyeBrush = new SolidBrush(Color.White))
            using (var pupilBrush = new SolidBrush(Color.Black))
            using (var beakBrush = new SolidBrush(Color.FromArgb(235, 95, 40)))
            {
                // Gövde
                g.FillEllipse(bodyBrush, -w / 2, -h / 2, w, h);
                g.FillEllipse(bellyBrush, -9f, 1f, 18f, 9f);
                g.DrawEllipse(outline, -w / 2, -h / 2, w, h);

                // Kanat (çırpma animasyonu)
                float wingOffset = IsDead ? 0f : (float)Math.Sin(wingTimer * 0.5f) * 5f;
                g.FillEllipse(wingBrush, -16f, -3f + wingOffset, 14f, 9f);
                g.DrawEllipse(outline, -16f, -3f + wingOffset, 14f, 9f);

                // Göz
                g.FillEllipse(eyeBrush, 4f, -11f, 11f, 11f);
                g.DrawEllipse(outline, 4f, -11f, 11f, 11f);
                g.FillEllipse(pupilBrush, 9f, -8f, 4f, 5f);

                // Gaga
                PointF[] beak =
                {
                    new PointF(10f, -1f),
                    new PointF(23f, 2f),
                    new PointF(10f, 7f)
                };
                g.FillPolygon(beakBrush, beak);
                g.DrawPolygon(outline, beak);
            }

            g.Restore(saved);
        }
    }
}
