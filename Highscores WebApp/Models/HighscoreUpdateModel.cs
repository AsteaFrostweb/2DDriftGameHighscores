using System.ComponentModel.DataAnnotations;

namespace Highscores_WebApp.Models
{
    public class HighscoreUpdateModel
    {     
        public string? MapName { get; set; }
        public float FastestLap { get; set; }
        public int BestComboScore { get; set; }
        public float BestComboTime { get; set; }
    }
}
