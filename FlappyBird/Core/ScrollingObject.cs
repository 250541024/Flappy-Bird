namespace FlappyBird.Core
{
    /// <summary>
    /// Sola doğru kayan nesnelerin soyut temel sınıfı.
    /// Kalıtım zinciri: GameObject → ScrollingObject → Obstacle → PipePair → MovingPipePair
    /// </summary>
    public abstract class ScrollingObject : GameObject, IScrollable
    {
        protected ScrollingObject(float x, float y, float width, float height, float parallaxFactor)
            : base(x, y, width, height)
        {
            ParallaxFactor = parallaxFactor;
        }

        public float ScrollSpeed { get; set; }

        /// <summary>
        /// 1 = oyun hızında kayar, 0.2 = daha yavaş kayar (uzaktaymış gibi görünür).
        /// </summary>
        protected float ParallaxFactor { get; }

        public override void Update(float dt)
        {
            X -= ScrollSpeed * ParallaxFactor * dt;

            if (X + Width < -10)
                OnLeftScreen();
        }

        /// <summary>
        /// Nesne ekranın solundan çıktığında çağrılır. Varsayılan davranış nesneyi
        /// pasif yapmaktır; alt sınıflar bunu değiştirebilir (ör. bulut yeniden doğar).
        /// </summary>
        protected virtual void OnLeftScreen()
        {
            IsActive = false;
        }
    }
}
