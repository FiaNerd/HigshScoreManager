using System.ComponentModel.DataAnnotations;

namespace HighScoreManager
{
    public class HighScore
    {
        public int Id { get; set; }

        [Required]
        public string GTitle { get; set; }

        [Required]
        public string Player { get; set; }

        public string Date { get; set; }

        [Required]
        public int Score { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }
    }
}
