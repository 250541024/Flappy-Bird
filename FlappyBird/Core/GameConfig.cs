namespace FlappyBird.Core
{
    /// <summary>
    /// Oyunun tüm sabit ayarları tek bir yerde toplanır.
    /// Bir değeri değiştirmek için sadece burayı düzenlemek yeterlidir.
    /// </summary>
    public static class GameConfig
    {
        // Ekran
        public const int ScreenWidth = 400;
        public const int ScreenHeight = 600;
        public const int GroundHeight = 80;
        public const int PlayAreaBottom = ScreenHeight - GroundHeight;

        // Kuş fiziği (birim: piksel / kare, 60 FPS'e göre)
        public const float Gravity = 0.45f;
        public const float FlapStrength = -7.8f;
        public const float MaxFallSpeed = 10f;
        public const float BirdStartX = ScreenWidth * 0.25f;
        public const float BirdStartY = ScreenHeight * 0.40f;

        // Borular ve zorluk
        public const float BaseSpeed = 2.6f;
        public const float MaxSpeed = 5.0f;
        public const float BaseGap = 165f;
        public const float MinGap = 115f;
        public const float PipeSpacing = 220f;
        public const float PipeMargin = 60f;
        public const int PointsPerLevel = 5;
        public const int MovingPipeLevel = 3;  // Bu seviyeden sonra hareketli borular çıkar
    }
}
