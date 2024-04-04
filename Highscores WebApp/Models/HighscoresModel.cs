using System.ComponentModel.DataAnnotations;

namespace Highscores_WebApp.Models
{
	public class HighscoresModel
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string? Player_Id { get; set; }
		[Required]
		public int Map_Id { get; set; }
		public float Fastest_Lap { get; set; }
		public int Best_Combo_Score { get; set; }
		public float Best_Combo_Time { get; set; }


		public HighscoreGetModel AsGetModel(string mapName, string userName) 
		{
			HighscoreGetModel model = new HighscoreGetModel();
			model.Fastest_Lap = TimeSpan.FromSeconds(Fastest_Lap).ToString(@"hh\:mm\:ss\.fff");
			model.Best_Combo_Time = TimeSpan.FromSeconds(Best_Combo_Time).ToString(@"hh\:mm\:ss\.fff");
			model.Map = mapName;
			model.Name = userName;
			model.Best_Combo_Score = Best_Combo_Score;

			return model;
        }
	}

}
