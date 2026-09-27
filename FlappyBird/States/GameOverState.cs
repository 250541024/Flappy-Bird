using System.Drawing;
using FlappyBird.Core;
using FlappyBird.Managers;
using FlappyBird.Rendering;

namespace FlappyBird.States
{
    /// <summary>Oyun sonu ekranı: skor, en yüksek skor ve madalya.</summary>
    public class GameOverState : GameState
    {
        private const float InputDelay = 30f;   // ~0.5 sn: yanlışlıkla yeniden başlamayı önler
        private const float PanelDelay = 20f;

        private float timer;
        private bool isNewRecord;

        public GameOverState(Game game) : base(game) { }

        public override void Enter()
        {
            Game.Bird.Kill();
            if (Game.Bird.IsOnGround(Game.Ground.Y))
                Game.Bird.LandOn(Game.Ground.Y);   // Zemine gömülmesin

            Game.Sound.PlayHit();
            Game.TriggerFlash();
            isNewRecord = Game.Score.SubmitHighScore();
        }

        public override void Update(float dt)
        {
            timer += dt;

            // Kuş yere düşene kadar düşmeye devam etsin
            float groundTop = Game.Ground.Y;
            if (!Game.Bird.IsOnGround(groundTop))
            {
                Game.Bird.Update(dt);
                if (Game.Bird.IsOnGround(groundTop))
                    Game.Bird.LandOn(groundTop);
            }
        }

        public override void HandleAction()
        {
            if (timer < InputDelay) return;

            Game.Reset();
            Game.ChangeState(new MenuState(Game));
        }

        public override void Draw(Graphics g)
        {
            float cx = GameConfig.ScreenWidth / 2f;

            if (timer < PanelDelay)
            {
                DrawHelper.DrawOutlinedText(g, Game.Score.Score.ToString(), 52f, cx, 40f, Color.White, 6f);
                return;
            }

            DrawHelper.DrawOutlinedText(g, "OYUN BİTTİ", 44f, cx, 110f, Color.FromArgb(250, 130, 50), 7f);

            var panel = new RectangleF(45, 190, GameConfig.ScreenWidth - 90, 170);
            DrawHelper.FillRoundedRect(g, panel, 16, DrawHelper.PanelColor, DrawHelper.PanelBorderColor);

            // Madalya
            DrawHelper.DrawSmallText(g, "MADALYA", 120, 210, Color.FromArgb(200, 170, 100));
            DrawMedal(g, Game.Score.GetMedal(), 120, 280, 34);

            // Skorlar
            Color labelColor = Color.FromArgb(215, 120, 60);
            DrawHelper.DrawSmallText(g, "SKOR", 270, 210, labelColor);
            DrawHelper.DrawOutlinedText(g, Game.Score.Score.ToString(), 32f, 270, 228, Color.White);
            DrawHelper.DrawSmallText(g, "EN İYİ", 270, 280, labelColor);
            DrawHelper.DrawOutlinedText(g, Game.Score.HighScore.ToString(), 32f, 270, 298, Color.White);

            if (isNewRecord)
                DrawHelper.DrawOutlinedText(g, "YENİ REKOR!", 22f, cx, 375f, Color.FromArgb(255, 80, 80), 4f);

            if (timer > InputDelay && ((int)(timer / 30f)) % 2 == 0)
                DrawHelper.DrawOutlinedText(g, "Tekrar için SPACE", 22f, cx, 420f, Color.White, 4f);
        }

        private static void DrawMedal(Graphics g, Medal medal, float cx, float cy, float radius)
        {
            Color color;
            switch (medal)
            {
                case Medal.Platinum: color = Color.FromArgb(225, 235, 245); break;
                case Medal.Gold: color = Color.FromArgb(255, 205, 50); break;
                case Medal.Silver: color = Color.FromArgb(200, 200, 210); break;
                case Medal.Bronze: color = Color.FromArgb(205, 127, 50); break;
                default: color = Color.FromArgb(215, 205, 180); break;
            }

            using (var brush = new SolidBrush(color))
            using (var pen = new Pen(DrawHelper.PanelBorderColor, 3f))
            {
                g.FillEllipse(brush, cx - radius, cy - radius, radius * 2, radius * 2);
                g.DrawEllipse(pen, cx - radius, cy - radius, radius * 2, radius * 2);
            }

            if (medal != Medal.None)
            {
                using (var shine = new SolidBrush(Color.FromArgb(150, 255, 255, 255)))
                {
                    g.FillEllipse(shine, cx - radius * 0.6f, cy - radius * 0.7f, radius * 0.6f, radius * 0.5f);
                }
            }
        }
    }
}
