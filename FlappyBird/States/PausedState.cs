using System.Drawing;
using FlappyBird.Core;
using FlappyBird.Rendering;

namespace FlappyBird.States
{
    /// <summary>
    /// Duraklatma ekranı. Duraklatılan durumu saklar; devam edildiğinde
    /// oyun kaldığı yerden sürer.
    /// </summary>
    public class PausedState : GameState
    {
        private readonly GameState resumeState;

        public PausedState(Game game, GameState resumeState) : base(game)
        {
            this.resumeState = resumeState;
        }

        public override void Update(float dt)
        {
            // Duraklatıldığında hiçbir şey hareket etmez
        }

        public override void HandleAction() { Resume(); }
        public override void HandlePause() { Resume(); }

        private void Resume()
        {
            Game.ChangeState(resumeState);
        }

        public override void Draw(Graphics g)
        {
            resumeState.Draw(g); // Skor görünmeye devam etsin

            float cx = GameConfig.ScreenWidth / 2f;
            DrawHelper.FillScreen(g, Color.FromArgb(120, 0, 0, 0), GameConfig.ScreenWidth, GameConfig.ScreenHeight);
            DrawHelper.DrawOutlinedText(g, "DURAKLATILDI", 36f, cx, 240f, Color.White, 6f);
            DrawHelper.DrawOutlinedText(g, "Devam için P veya SPACE", 18f, cx, 295f, Color.White, 4f);
        }
    }
}
