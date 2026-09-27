using System;
using System.Collections.Generic;
using System.Drawing;
using FlappyBird.Entities;
using FlappyBird.Managers;
using FlappyBird.Rendering;
using FlappyBird.States;

namespace FlappyBird.Core
{
    /// <summary>
    /// Oyunun beyni. Tüm nesneleri (kuş, engeller, arka plan) ve yöneticileri
    /// (skor, zorluk, ses) bir arada tutar. Hangi ekranda olunduğunu ise
    /// GameState nesnesine bırakır (State tasarım deseni).
    /// </summary>
    public class Game : IDisposable
    {
        private readonly Random random = new Random();
        private readonly Background background = new Background();
        private readonly List<Cloud> clouds = new List<Cloud>();
        private readonly List<Obstacle> obstacles = new List<Obstacle>();

        private GameState currentState;
        private float flashAlpha;

        public Game()
        {
            for (int i = 0; i < 5; i++)
                clouds.Add(Cloud.CreateRandom());

            // OLAY ABONELİĞİ: puan kazanılınca ses çal ve zorluğu güncelle
            Score.PointScored += Score_PointScored;

            Reset();
            ChangeState(new MenuState(this));
        }

        /// <summary>Kullanıcı menüdeyken ESC'ye basınca tetiklenir; form kendini kapatır.</summary>
        public event EventHandler ExitRequested;

        public Bird Bird { get; private set; }
        public Ground Ground { get; } = new Ground();
        public ScoreManager Score { get; } = new ScoreManager();
        public DifficultyManager Difficulty { get; } = new DifficultyManager();
        public SoundManager Sound { get; } = new SoundManager();

        // ------------------------------------------------------------------
        //  Nesne koleksiyonları (POLİMORFİZM için ortak türler üzerinden)
        // ------------------------------------------------------------------

        /// <summary>Çizim sırasına göre (arkadan öne) tüm oyun nesneleri.</summary>
        private IEnumerable<GameObject> WorldObjects
        {
            get
            {
                yield return background;
                foreach (Cloud c in clouds) yield return c;
                foreach (Obstacle o in obstacles) yield return o;
                yield return Ground;
                yield return Bird;
            }
        }

        private IEnumerable<IScrollable> Scrollables
        {
            get
            {
                yield return background;
                foreach (Cloud c in clouds) yield return c;
                foreach (Obstacle o in obstacles) yield return o;
                yield return Ground;
            }
        }

        private IEnumerable<ICollidable> Collidables
        {
            get
            {
                foreach (Obstacle o in obstacles) yield return o;
                yield return Ground;
            }
        }

        // ------------------------------------------------------------------
        //  Durum yönetimi
        // ------------------------------------------------------------------

        public void ChangeState(GameState newState)
        {
            currentState = newState;
            currentState.Enter();
        }

        /// <summary>Yeni bir oyun için her şeyi başlangıç değerlerine döndürür.</summary>
        public void Reset()
        {
            Bird = new Bird(GameConfig.BirdStartX, GameConfig.BirdStartY);
            obstacles.Clear();
            Score.Reset();
            Difficulty.Reset();
            flashAlpha = 0;
        }

        // ------------------------------------------------------------------
        //  Durumların kullandığı oyun mantığı
        // ------------------------------------------------------------------

        /// <summary>Arka plan, bulutlar ve zemini kaydırır.</summary>
        public void UpdateScenery(float dt)
        {
            foreach (IScrollable s in Scrollables)
                s.ScrollSpeed = Difficulty.Speed;

            background.Update(dt);
            foreach (Cloud c in clouds) c.Update(dt);
            Ground.Update(dt);
        }

        /// <summary>Engelleri hareket ettirir, ekrandan çıkanları siler, yenilerini ekler.</summary>
        public void UpdateObstacles(float dt)
        {
            // POLİMORFİZM: liste Obstacle türünde ama içinde PipePair ve
            // MovingPipePair var. Her biri kendi Update metodunu çalıştırır.
            foreach (Obstacle o in obstacles) o.Update(dt);
            obstacles.RemoveAll(o => !o.IsActive);

            if (obstacles.Count == 0)
                SpawnObstacle(GameConfig.ScreenWidth + 100);
            else if (obstacles[obstacles.Count - 1].X < GameConfig.ScreenWidth - GameConfig.PipeSpacing)
                SpawnObstacle(GameConfig.ScreenWidth);
        }

        private void SpawnObstacle(float x)
        {
            float gap = Difficulty.GapSize;
            bool moving = Difficulty.ShouldSpawnMovingPipe(random);
            float amplitude = moving ? 40f : 0f;

            float minY = GameConfig.PipeMargin + gap / 2 + amplitude;
            float maxY = GameConfig.PlayAreaBottom - GameConfig.PipeMargin - gap / 2 - amplitude;
            float gapCenter = minY + (float)random.NextDouble() * (maxY - minY);

            Obstacle obstacle = moving
                ? new MovingPipePair(x, gapCenter, gap, amplitude)
                : new PipePair(x, gapCenter, gap);

            obstacle.ScrollSpeed = Difficulty.Speed;
            obstacles.Add(obstacle);
        }

        /// <summary>Kuşun geçtiği engeller için puan verir.</summary>
        public void CheckPassedObstacles()
        {
            foreach (Obstacle o in obstacles)
            {
                if (o.TryPass(Bird.X))
                    Score.AddPoint();
            }
        }

        /// <summary>Kuş herhangi bir engele veya zemine çarptı mı?</summary>
        public bool CheckCollision()
        {
            RectangleF birdBounds = Bird.Bounds;
            foreach (ICollidable c in Collidables)
            {
                if (c.CollidesWith(birdBounds))
                    return true;
            }
            return false;
        }

        public void TriggerFlash()
        {
            flashAlpha = 200f;
        }

        private void Score_PointScored(object sender, int newScore)
        {
            Sound.PlayPoint();
            Difficulty.Update(newScore);
        }

        // ------------------------------------------------------------------
        //  Form'dan gelen çağrılar
        // ------------------------------------------------------------------

        public void Update(float dt)
        {
            if (flashAlpha > 0) flashAlpha = Math.Max(0f, flashAlpha - 12f * dt);
            currentState.Update(dt);
        }

        public void Draw(Graphics g)
        {
            // POLİMORFİZM: her nesne kendi Draw metodunu çalıştırır
            foreach (GameObject obj in WorldObjects)
                obj.Draw(g);

            currentState.Draw(g);

            // Ses durumu (zeminin üzerinde)
            string soundText = Sound.IsMuted ? "Ses: KAPALI (M)" : "Ses: AÇIK (M)";
            using (var brush = new SolidBrush(Color.FromArgb(110, 90, 50)))
            {
                g.DrawString(soundText, DrawHelper.SmallFont, brush, 10, GameConfig.ScreenHeight - 26);
            }

            // Çarpma anındaki beyaz flaş
            if (flashAlpha > 0)
            {
                DrawHelper.FillScreen(g, Color.FromArgb((int)flashAlpha, 255, 255, 255),
                                      GameConfig.ScreenWidth, GameConfig.ScreenHeight);
            }
        }

        public void OnAction() { currentState.HandleAction(); }
        public void OnPause() { currentState.HandlePause(); }
        public void ToggleMute() { Sound.IsMuted = !Sound.IsMuted; }

        public void OnEscape()
        {
            if (currentState is MenuState)
            {
                ExitRequested?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Reset();
                ChangeState(new MenuState(this));
            }
        }

        public void Dispose()
        {
            Score.PointScored -= Score_PointScored;
            Sound.Dispose();
        }
    }
}
