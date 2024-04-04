using Highscores_WebApp.Data;
using Highscores_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
public class HighscoresController : Controller
{
	private readonly ApplicationDbContext _context;

	public HighscoresController(ApplicationDbContext context)
	{
		_context = context;
	}

    [AllowAnonymous]
    public IActionResult Index(string sortOrder)
    {
        // Retrieve highscores, maps, and users
        var highscores = _context.Highscores.ToList();

        // Sorting logic based on sortOrder parameter
        switch (sortOrder)
        {
            case "FastestLap_ASC":
                highscores = highscores.OrderBy(h => h.Fastest_Lap).ToList();
                break;
            case "FastestLap_DESC":
                highscores = highscores.OrderByDescending(h => h.Fastest_Lap).ToList();
                break;
            case "BestComboScore_ASC":
                highscores = highscores.OrderBy(h => h.Best_Combo_Score).ToList();
                break;
            case "BestComboScore_DESC":
                highscores = highscores.OrderByDescending(h => h.Best_Combo_Score).ToList();
                break;
            case "BestComboTime_ASC":
                highscores = highscores.OrderBy(h => h.Best_Combo_Time).ToList();
                break;
            case "BestComboTime_DESC":
                highscores = highscores.OrderByDescending(h => h.Best_Combo_Time).ToList();
                break;
            default:
                // Default sorting criteria if sortOrder parameter is not specified
                highscores = highscores.OrderBy(h => h.Fastest_Lap).ToList();
                break;
        }

        var viewModel = new HighscoresViewModel
        {
            Highscores = highscores,
            Maps = _context.Maps.ToList(),
            Users = _context.Users.ToList()
        };

        return View(viewModel); // Pass HighscoresViewModel instance to the view
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetHighScores(string MapName, int ToRank, string UserName)
    {
        
        
        //Validate inputs
        if (ToRank <= 0) 
        {
            return BadRequest("Must request more than 0 higshcores");
        }
        if (MapName == null) 
        {
            return BadRequest("Map name is required.");          
        }       

        //Finding the map from name to get the ID and to check it exists
        MapsModel map_model = _context.Maps.FirstOrDefault(m => m.Name == MapName);
        if (map_model == null)
        {
            return BadRequest("Unable to find map with the name specified");
        }
        int map_Id = map_model.Id;

        var HighScoresQuery = _context.Highscores.Where(h => h.Map_Id == map_Id);
        if (HighScoresQuery.Count<HighscoresModel>() == 0) 
        {
            return BadRequest("Unable to find any highsores for the map specified");
        }

        var users = _context.Users;

        //if the request is for all users
        if (UserName == null)
        {           
            List<HighscoresModel> fastest_combos = HighScoresQuery.OrderBy(h => h.Fastest_Lap).Take(ToRank).ToList();
            List<HighscoresModel> longest_combos = HighScoresQuery.OrderByDescending(h => h.Best_Combo_Time).Take(ToRank).ToList();
            List<HighscoresModel> best_combo_scores = HighScoresQuery.OrderByDescending(h => h.Best_Combo_Score).Take(ToRank).ToList();

            List<HighscoresModel> highscores = new List<HighscoresModel>();
            for (int i = 0; i < ToRank; i++)
            {
                if (!highscores.Contains(fastest_combos[i])) highscores.Add(fastest_combos[i]);
                if (!highscores.Contains(longest_combos[i])) highscores.Add(longest_combos[i]);
                if (!highscores.Contains(best_combo_scores[i])) highscores.Add(best_combo_scores[i]);
            }

            List<HighscoreGetModel> get_highscores = new List<HighscoreGetModel>();
            foreach (var highscore in highscores)
            {
                IdentityUser? user = users.SingleOrDefault(u => u.Id == highscore.Player_Id);
                if (user != null)
                {
                    get_highscores.Add(highscore.AsGetModel(MapName, user.UserName));
                }
            }

            return Ok(get_highscores);
        }
        else
        {
            IdentityUser user_model = _context.Users.FirstOrDefault(u => u.UserName == UserName);
            if (user_model == null)
            {
                return BadRequest("Unable to find user with the name specified");
            }
            string user_Id = user_model.Id;

            //if the request is for only a single user
            HighscoresModel? HighScore = HighScoresQuery.FirstOrDefault(h => h.Map_Id == map_Id && h.Player_Id == user_Id);
            if (HighScore == null) 
            {
                return BadRequest("Failed to find player highscores");
            }

            List<HighscoreGetModel> get_highscores = new List<HighscoreGetModel>();
            get_highscores.Add(HighScore.AsGetModel(MapName, UserName));
            return Ok(get_highscores);
        }

    }

    [Authorize]
    [HttpPost]
    public IActionResult UpdateHighScore(HighscoreUpdateModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var map = _context.Maps.FirstOrDefault(m => m.Name == model.MapName);
        int map_id;
        if (map == null)
        {
            return BadRequest("Map information is missing.");
        }
        else
        {
            map_id = map.Id;
        }

        // Check if the user has an existing high score for the specified map
        var existingHighScore = _context.Highscores.FirstOrDefault(h => h.Player_Id == userId && h.Map_Id == map_id);
     
      
        if (existingHighScore == null)
        {
            // If no existing high score found, create a new one
            var newHighScore = new HighscoresModel
            {
                Player_Id = userId,
                Map_Id = map_id,
                Fastest_Lap = model.FastestLap,
                Best_Combo_Score = model.BestComboScore,
                Best_Combo_Time = model.BestComboTime
            };

            _context.Highscores.Add(newHighScore);
        }
        else
        {
            // If an existing high score found, update its values

            existingHighScore.Fastest_Lap = model.FastestLap;
            existingHighScore.Best_Combo_Score = model.BestComboScore;
            existingHighScore.Best_Combo_Time = model.BestComboTime;
        }

        _context.SaveChanges();

        return Ok();
    }

}