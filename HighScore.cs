namespace HighScoreManager
{
    public class HighScore
    {
        public int Id { get; set; }

        public string GTitle { get; set; }

        public string Player { get; set; }

        public string Date { get; set; }

        public int Score { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }
    }
}
