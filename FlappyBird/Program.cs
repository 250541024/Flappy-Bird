using System;
using System.Windows.Forms;

namespace FlappyBird
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın giriş noktası.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
