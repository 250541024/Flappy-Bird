using System;
using FlappyBird.Core;

namespace FlappyBird.Managers
{
    /// <summary>
    /// Skora göre oyunun zorluğunu ayarlar: her 5 puanda seviye artar,
    /// borular hızlanır, boşluk daralır ve 3. seviyeden sonra hareketli borular çıkar.
    /// </summary>
    public class DifficultyManager
    {
        public DifficultyManager()
        {
            Reset();
        }

        public int Level { get; private set; }
        public float Speed { get; private set; }
        public float GapSize { get; private set; }

        public void Reset()
        {
            Level = 1;
            Speed = GameConfig.BaseSpeed;
            GapSize = GameConfig.BaseGap;
        }

        public void Update(int score)
        {
            int steps = score / GameConfig.PointsPerLevel;
            Level = steps + 1;
            Speed = Math.Min(GameConfig.MaxSpeed, GameConfig.BaseSpeed + steps * 0.3f);
            GapSize = Math.Max(GameConfig.MinGap, GameConfig.BaseGap - steps * 7f);
        }

        public bool ShouldSpawnMovingPipe(Random random)
        {
            return Level >= GameConfig.MovingPipeLevel && random.NextDouble() < 0.4;
        }
    }
}
