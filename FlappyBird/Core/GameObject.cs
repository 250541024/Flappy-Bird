using System.Drawing;

namespace FlappyBird.Core
{
    /// <summary>
    /// SOYUT (abstract) TEMEL SINIF.
    /// Ekranda görünen her nesnenin ortak özelliklerini (konum, boyut) ve
    /// ortak davranışlarını (güncellenme, çizilme) tanımlar.
    /// Bu sınıftan doğrudan nesne oluşturulamaz; Bird, PipePair, Cloud gibi
    /// alt sınıflar Update ve Draw metotlarını kendilerine göre yazmak ZORUNDADIR.
    /// </summary>
    public abstract class GameObject
    {
        protected GameObject(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            IsActive = true;
        }

        // KAPSÜLLEME: Konum dışarıdan okunabilir ama sadece nesnenin kendisi
        // ve alt sınıfları değiştirebilir.
        public float X { get; protected set; }
        public float Y { get; protected set; }
        public float Width { get; }
        public float Height { get; }

        /// <summary>false olduğunda nesne oyundan kaldırılır.</summary>
        public bool IsActive { get; protected set; }

        /// <summary>
        /// Nesnenin kapladığı alan. virtual olduğu için alt sınıflar
        /// (ör. Bird) daha küçük bir çarpışma kutusu tanımlayabilir.
        /// </summary>
        public virtual RectangleF Bounds
        {
            get { return new RectangleF(X, Y, Width, Height); }
        }

        /// <summary>Her karede nesnenin durumunu günceller.</summary>
        public abstract void Update(float dt);

        /// <summary>Nesneyi ekrana çizer.</summary>
        public abstract void Draw(Graphics g);
    }
}
