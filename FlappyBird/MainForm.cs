using System;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using FlappyBird.Core;

namespace FlappyBird
{
    /// <summary>
    /// Ana pencere. Sadece üç iş yapar: zamanlayıcıyı çalıştırır, klavye/fare
    /// girdisini Game nesnesine iletir ve ekranı çizdirir. Oyun mantığı burada değil,
    /// Game sınıfındadır (Sorumlulukların ayrılması - Separation of Concerns).
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly Game game;
        private readonly Stopwatch clock = new Stopwatch();
        private long lastTicks;

        public MainForm()
        {
            InitializeComponent();

            // Titremeyi önlemek için çift tamponlu çizim
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            game = new Game();
            game.ExitRequested += Game_ExitRequested; // Olay (event) aboneliği

            clock.Start();
            lastTicks = clock.ElapsedTicks;
            gameTimer.Start();
        }

        private void Game_ExitRequested(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>Oyun döngüsü: yaklaşık 60 kez/saniye çalışır.</summary>
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            // dt = önceki kareden bu yana geçen süre ("60 FPS'lik kare" cinsinden).
            // Böylece oyun hızı bilgisayarın hızına bağlı olmaz.
            long now = clock.ElapsedTicks;
            float dt = (now - lastTicks) / (float)Stopwatch.Frequency * 60f;
            lastTicks = now;
            if (dt > 3f) dt = 3f;

            game.Update(dt);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            game.Draw(e.Graphics);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Space:
                case Keys.W:
                case Keys.Enter:
                    game.OnAction();
                    break;
                case Keys.P:
                    game.OnPause();
                    break;
                case Keys.M:
                    game.ToggleMute();
                    break;
                case Keys.Escape:
                    game.OnEscape();
                    break;
                default:
                    return;
            }

            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        /// <summary>Yukarı ok tuşu KeyDown'a ulaşmadan yakalandığı için burada işlenir.</summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                game.OnAction();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left) game.OnAction();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            gameTimer.Stop();
            game.Dispose();
            base.OnFormClosed(e);
        }
    }
}
