using System.Drawing;

namespace FlappyBird.Core
{
    /// <summary>
    /// ARAYÜZ (interface): Kuşun çarpabileceği her nesne bu sözleşmeyi uygular.
    /// Borular ve zemin birbirinden çok farklı sınıflar olsa da Game sınıfı
    /// hepsini "ICollidable" olarak aynı şekilde kontrol eder.
    /// </summary>
    public interface ICollidable
    {
        bool CollidesWith(RectangleF area);
    }
}
