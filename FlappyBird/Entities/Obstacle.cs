using System.Drawing;
using FlappyBird.Core;

namespace FlappyBird.Entities
{
    /// <summary>
    /// Tüm engellerin SOYUT temel sınıfı. Hem kayan bir nesnedir (ScrollingObject'ten
    /// kalıtım) hem de çarpılabilir bir nesnedir (ICollidable arayüzünü uygular).
    /// Yeni bir engel türü eklemek için bu sınıftan türetmek yeterlidir.
    /// </summary>
    public abstract class Obstacle : ScrollingObject, ICollidable
    {
        protected Obstacle(float x, float width)
            : base(x, 0, width, GameConfig.PlayAreaBottom, 1f)
        {
        }

        /// <summary>Kuş bu engeli geçti mi? (Puan bir kez verilsin diye)</summary>
        public bool IsPassed { get; private set; }

        /// <summary>
        /// Kuş engelin sağ kenarını geçtiyse true döner (sadece ilk seferde).
        /// </summary>
        public bool TryPass(float birdX)
        {
            if (!IsPassed && X + Width < birdX)
            {
                IsPassed = true;
                return true;
            }
            return false;
        }

        // Her engel türü çarpışmayı kendi şekline göre hesaplar.
        public abstract bool CollidesWith(RectangleF area);
    }
}
