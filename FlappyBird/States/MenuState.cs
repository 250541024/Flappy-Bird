using System.Drawing;
using FlappyBird.Core;
using FlappyBird.Rendering;

namespace FlappyBird.States
{
    /// <summary>Başlangıç ekranı: başlık, kontroller ve en yüksek skor.</summary>
    public class MenuState : GameState
    {
        private float time;

        public MenuState(Game game) : base(game) { }

        public override void Update(float dt)
        {
            time += dt;
            Game.Bird.Hover(time, dt);
            Game.UpdateScenery(dt);
        }

        public override void HandleAction()
        {
            Game.ChangeState(new PlayingState(Game));
            Game.Bird.Flap();
            Game.Sound.PlayFlap();
        }

        public override void Draw(Graphics g)
        {
            float cx = GameConfig.ScreenWidth / 2f;

            DrawHelper.DrawOutlinedText(g, "FLAPPY BIRD", 46f, cx, 70f, Color.FromArgb(255, 215, 60), 7f);
            DrawHelper.DrawOutlinedText(g, "Hazır ol!", 28f, cx, 140f, Color.FromArgb(240, 120, 60), 5f);

            var panel = new RectangleF(60, 330, GameConfig.ScreenWidth - 120, 120);
            DrawHelper.FillRoundedRect(g, panel, 14, Color.FromArgb(225, 255, 250, 225), DrawHelper.PanelBorderColor);

            Color textColor = Color.FromArgb(90, 65, 30);
            DrawHelper.DrawSmallText(g, "SPACE / ↑ / Tıkla : Zıpla", cx, 345, textColor);
            DrawHelper.DrawSmallText(g, "P : Duraklat     M : Ses", cx, 370, textColor);
            DrawHelper.DrawSmallText(g, "ESC : Menü / Çıkış", cx, 395, textColor);
            DrawHelper.DrawSmallText(g, "En Yüksek Skor: " + Game.Score.HighScore, cx, 422, textColor);

            // Yanıp sönen yazı
            if (((int)(time / 30f)) % 2 == 0)
                DrawHelper.DrawOutlinedText(g, "Başlamak için SPACE", 20f, cx, 470f, Color.White, 4f);
        }
    }
}
