namespace FlappyBird.Core
{
    /// <summary>
    /// ARAYÜZ: Oyun hızıyla birlikte sola doğru kayan nesneler
    /// (borular, bulutlar, arka plan, zemin) bu arayüzü uygular.
    /// Zorluk arttığında Game, hepsinin hızını tek döngüyle günceller.
    /// </summary>
    public interface IScrollable
    {
        float ScrollSpeed { get; set; }
    }
}
