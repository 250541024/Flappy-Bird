# Flappy Bird: Nesne Tabanlı Programlama Ödevi

C# ve Windows Forms ile yazılmış bir Flappy Bird oyunu. Harici görsel veya ses dosyası kullanılmıyor; her şey kodla çiziliyor.

## Çalıştırma

1. Visual Studio'da `FlappyBird.sln` dosyasını açın.
2. **F5** tuşuna basın ya da üstteki yeşil ▶ **FlappyBird** düğmesine tıklayın.

Gereken: Visual Studio 2026, ".NET masaüstü geliştirme" iş yükü (.NET 10).

## Kontroller

| Tuş | İşlev |
|---|---|
| SPACE / ↑ / W / Sol tık | Zıpla (menüdeyken oyunu başlatır) |
| P | Duraklat / devam et |
| M | Sesi aç / kapat |
| ESC | Menüye dön (menüdeyken çıkış) |

## Klasör Yapısı

```
FlappyBird/
├── Program.cs              Giriş noktası
├── MainForm.cs             Pencere: zamanlayıcı, girdi, çizim çağrısı
├── MainForm.Designer.cs    Form tasarımcısı kodu
├── Core/
│   ├── GameObject.cs       Soyut temel sınıf (tüm oyun nesneleri)
│   ├── ScrollingObject.cs  Soyut sınıf (sola kayan nesneler)
│   ├── ICollidable.cs      Arayüz (çarpılabilir nesneler)
│   ├── IScrollable.cs      Arayüz (kayan nesneler)
│   ├── GameConfig.cs       Sabit ayarlar
│   └── Game.cs             Oyunun ana yöneticisi
├── Entities/
│   ├── Bird.cs             Kuş
│   ├── Obstacle.cs         Soyut engel sınıfı
│   ├── PipePair.cs         Klasik boru çifti
│   ├── MovingPipePair.cs   Hareketli boru (3. seviyeden sonra)
│   ├── Cloud.cs            Bulut
│   ├── Background.cs       Gökyüzü ve tepeler
│   └── Ground.cs           Zemin
├── States/                 State (Durum) tasarım deseni
│   ├── GameState.cs        Soyut durum sınıfı
│   ├── MenuState.cs
│   ├── PlayingState.cs
│   ├── PausedState.cs
│   └── GameOverState.cs
├── Managers/
│   ├── ScoreManager.cs     Skor, rekor kaydı, madalya, olay (event)
│   ├── DifficultyManager.cs Seviye, hız, boşluk
│   └── SoundManager.cs     Kodla üretilen sesler
└── Rendering/
    └── DrawHelper.cs       Ortak çizim metotları
```

## Sınıf Hiyerarşisi (Kalıtım)

```
GameObject (abstract)
├── Bird
├── Background          : IScrollable
├── Ground              : ICollidable, IScrollable
└── ScrollingObject (abstract) : IScrollable
    ├── Cloud
    └── Obstacle (abstract) : ICollidable
        └── PipePair
            └── MovingPipePair

GameState (abstract)
├── MenuState
├── PlayingState
├── PausedState
└── GameOverState
```

## Kullanılan OOP Kavramları

| Kavram | Koddaki Örneği |
|---|---|
| **Kapsülleme (Encapsulation)** | `GameObject.X / Y` dışarıdan okunur ama sadece `protected set` ile değiştirilir. `ScoreManager.Score` yalnızca `AddPoint()` ile artar. |
| **Kalıtım (Inheritance)** | Dört seviyeli zincir: `GameObject → ScrollingObject → Obstacle → PipePair → MovingPipePair` |
| **Soyutlama (Abstraction)** | `GameObject`, `ScrollingObject`, `Obstacle`, `GameState` soyut sınıflardır; `Update`, `Draw`, `CollidesWith` soyut metotlardır. |
| **Çok biçimlilik (Polymorphism)** | `Game.Draw` tüm nesneleri `GameObject` olarak dolaşır ve her biri kendi `Draw` metodunu çalıştırır. Engel listesi `List<Obstacle>` türündedir ama içinde hem `PipePair` hem `MovingPipePair` bulunur. |
| **Metot ezme (override) ve `base` çağrısı** | `MovingPipePair.Update` önce `base.Update(dt)` çağırır, sonra boşluğu hareket ettirir. `Bird.Bounds`, `Cloud.OnLeftScreen` de override edilir. |
| **Arayüz (Interface)** | `ICollidable` (borular ve zemin), `IScrollable` (kayan her şey). `Ground` iki arayüzü birden uygular. |
| **Olay (Event)** | `ScoreManager.PointScored` olayına `Game` abone olur (ses çalar, zorluğu artırır). `Game.ExitRequested` olayına `MainForm` abone olur. |
| **Tasarım deseni** | **State**: her ekran ayrı bir sınıftır, `switch-case` yerine polimorfizm kullanılır. **Factory method**: `Cloud.CreateRandom()` |
| **IDisposable** | `SoundManager` ve `Game`, kaynaklarını `Dispose()` ile serbest bırakır. |
| **Enum** | `Medal { None, Bronze, Silver, Gold, Platinum }` |

## Oyun Özellikleri

- Menü, oyun, duraklatma ve oyun sonu ekranları
- Yerçekimi fiziği, hıza göre dönen kuş, kanat animasyonu
- Her 5 puanda bir seviye atlanıyor: borular hızlanıyor, aradaki boşluk daralıyor
- 3. seviyeden sonra turuncu, yukarı aşağı hareket eden borular çıkıyor
- En yüksek skor dosyaya kaydediliyor (`%AppData%\FlappyBird\highscore.txt`)
- Madalyalar: 10 puanda bronz, 20'de gümüş, 30'da altın, 40'ta platin
- Kodla üretilen ses efektleri
- Paralaks arka plan: bulutlar, tepeler ve zemin farklı hızlarda kayıyor

## Oyun Döngüsü

1. `MainForm` içindeki `Timer` her ~15 ms'de `GameTimer_Tick` metodunu çağırır.
2. `Stopwatch` ile geçen süre (`dt`) hesaplanır. Böylece oyun her bilgisayarda aynı hızda akar.
3. `game.Update(dt)` çağrılır. Bu çağrı mevcut duruma (`currentState.Update`) iletilir.
4. `Invalidate()` ekranı yeniden çizdirir. `OnPaint` içinde `game.Draw(g)` önce dünyayı, sonra durumun arayüzünü çizer.
