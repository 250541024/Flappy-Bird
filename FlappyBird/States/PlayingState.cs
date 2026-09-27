using System.Drawing;
using FlappyBird.Core;
using FlappyBird.Rendering;

namespace FlappyBird.States
{
    /// <summary>Asıl oyunun oynandığı durum.</summary>
    public class PlayingState : GameState
    {
        public PlayingState(Game game) : base(game) { }

        public override void Update(float dt)
        {
            Game.UpdateScenery(dt);
            Game.Bird.Update(dt);
            Game.UpdateObstacles(dt);
            Game.CheckPassedObstacles();

            if (Game.CheckCollision())
                Game.ChangeState(new GameOverState(Game));
        }

        public override void HandleAction()
        {
            Game.Bird.Flap();
            Game.Sound.PlayFlap();
        }

        public override void HandlePause()
        {
            // Mevcut durumu (this) saklayarak duraklat; devam edince aynen geri döner
            Game.ChangeState(new PausedState(Game, this));
        }

        public override void Draw(Graphics g)
        {
            float cx = GameConfig.ScreenWidth / 2f;
            DrawHelper.DrawOutlinedText(g, Game.Score.Score.ToString(), 52f, cx, 40f, Color.White, 6f);
            DrawHelper.DrawOutlinedText(g, "Seviye " + Game.Difficulty.Level, 14f, cx, 105f, Color.White, 3f);
        }
    }
}
