using System.Drawing;
using FlappyBird.Core;

namespace FlappyBird.States
{
    /// <summary>
    /// STATE (DURUM) TASARIM DESENİ - soyut temel sınıf.
    /// Oyunun her ekranı (Menü, Oyun, Duraklatma, Oyun Sonu) bu sınıftan türetilir.
    /// Game sınıfı hangi durumda olduğunu bilmeden sadece currentState.Update(),
    /// currentState.Draw() gibi metotları çağırır; doğru davranışı alt sınıf belirler.
    /// Bu sayede uzun switch-case blokları yerine temiz, ayrık sınıflar elde edilir.
    /// </summary>
    public abstract class GameState
    {
        protected GameState(Game game)
        {
            Game = game;
        }

        protected Game Game { get; }

        /// <summary>Duruma geçildiğinde bir kez çalışır.</summary>
        public virtual void Enter() { }

        public abstract void Update(float dt);

        /// <summary>Oyun dünyasının üzerine bu ekrana özel arayüzü çizer.</summary>
        public abstract void Draw(Graphics g);

        /// <summary>Space / Yukarı ok / Enter / fare tıklaması.</summary>
        public abstract void HandleAction();

        /// <summary>P tuşu. Varsayılan olarak hiçbir şey yapmaz.</summary>
        public virtual void HandlePause() { }
    }
}
