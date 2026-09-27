using System;
using System.IO;

namespace FlappyBird.Managers
{
    public enum Medal
    {
        None,
        Bronze,
        Silver,
        Gold,
        Platinum
    }

    /// <summary>
    /// Skoru tutar, en yüksek skoru dosyaya kaydeder/okur ve puan kazanıldığında
    /// PointScored OLAYINI (event) tetikler.
    /// Dosya: %AppData%\FlappyBird\highscore.txt
    /// </summary>
    public class ScoreManager
    {
        private readonly string folderPath;
        private readonly string filePath;

        public ScoreManager()
        {
            folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FlappyBird");
            filePath = Path.Combine(folderPath, "highscore.txt");
            LoadHighScore();
        }

        /// <summary>Her puan kazanıldığında tetiklenir. Parametre: yeni skor.</summary>
        public event EventHandler<int> PointScored;

        public int Score { get; private set; }
        public int HighScore { get; private set; }

        public void AddPoint()
        {
            Score++;
            PointScored?.Invoke(this, Score);
        }

        public void Reset()
        {
            Score = 0;
        }

        public Medal GetMedal()
        {
            if (Score >= 40) return Medal.Platinum;
            if (Score >= 30) return Medal.Gold;
            if (Score >= 20) return Medal.Silver;
            if (Score >= 10) return Medal.Bronze;
            return Medal.None;
        }

        /// <summary>Skor rekor ise kaydeder ve true döner.</summary>
        public bool SubmitHighScore()
        {
            if (Score <= HighScore) return false;

            HighScore = Score;
            try
            {
                Directory.CreateDirectory(folderPath);
                File.WriteAllText(filePath, HighScore.ToString());
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            return true;
        }

        private void LoadHighScore()
        {
            try
            {
                if (File.Exists(filePath) &&
                    int.TryParse(File.ReadAllText(filePath).Trim(), out int value) && value > 0)
                {
                    HighScore = value;
                }
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }
}
