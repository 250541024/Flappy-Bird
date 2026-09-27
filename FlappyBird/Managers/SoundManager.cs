using System;
using System.IO;
using System.Media;
using System.Text;

namespace FlappyBird.Managers
{
    /// <summary>
    /// Ses efektleri. Harici dosya gerekmemesi için sesler çalışma anında
    /// sinüs dalgasından WAV formatında üretilir. IDisposable uygular çünkü
    /// SoundPlayer nesneleri kapatılırken serbest bırakılmalıdır.
    /// </summary>
    public class SoundManager : IDisposable
    {
        private const int SampleRate = 22050;

        private readonly SoundPlayer flapSound;
        private readonly SoundPlayer pointSound;
        private readonly SoundPlayer hitSound;

        public SoundManager()
        {
            flapSound = CreatePlayer(CreateTone(520f, 780f, 90, 0.35f));
            pointSound = CreatePlayer(CreateTone(880f, 1400f, 140, 0.30f));
            hitSound = CreatePlayer(CreateTone(300f, 80f, 280, 0.45f));
        }

        public bool IsMuted { get; set; }

        public void PlayFlap() { Play(flapSound); }
        public void PlayPoint() { Play(pointSound); }
        public void PlayHit() { Play(hitSound); }

        private void Play(SoundPlayer player)
        {
            if (IsMuted || player == null) return;
            try
            {
                player.Play(); // Asenkron: oyunu bekletmez
            }
            catch (Exception)
            {
                // Ses cihazı yoksa oyun sessiz devam etsin
            }
        }

        private static SoundPlayer CreatePlayer(byte[] wavData)
        {
            try
            {
                var player = new SoundPlayer(new MemoryStream(wavData));
                player.Load();
                return player;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Frekansı startFreq'ten endFreq'e kayan, sönümlenen bir ton üretir (16-bit mono WAV).
        /// </summary>
        private static byte[] CreateTone(float startFreq, float endFreq, int durationMs, float volume)
        {
            int sampleCount = SampleRate * durationMs / 1000;
            int dataSize = sampleCount * 2;

            using (var ms = new MemoryStream())
            using (var w = new BinaryWriter(ms))
            {
                // WAV başlığı
                w.Write(Encoding.ASCII.GetBytes("RIFF"));
                w.Write(36 + dataSize);
                w.Write(Encoding.ASCII.GetBytes("WAVE"));
                w.Write(Encoding.ASCII.GetBytes("fmt "));
                w.Write(16);
                w.Write((short)1);          // PCM
                w.Write((short)1);          // Mono
                w.Write(SampleRate);
                w.Write(SampleRate * 2);    // Byte rate
                w.Write((short)2);          // Block align
                w.Write((short)16);         // Bit derinliği
                w.Write(Encoding.ASCII.GetBytes("data"));
                w.Write(dataSize);

                double phase = 0;
                for (int i = 0; i < sampleCount; i++)
                {
                    double t = (double)i / sampleCount;
                    double freq = startFreq + (endFreq - startFreq) * t;
                    phase += 2 * Math.PI * freq / SampleRate;
                    double envelope = 1.0 - t;
                    w.Write((short)(Math.Sin(phase) * volume * envelope * short.MaxValue));
                }

                w.Flush();
                return ms.ToArray();
            }
        }

        public void Dispose()
        {
            flapSound?.Dispose();
            pointSound?.Dispose();
            hitSound?.Dispose();
        }
    }
}
