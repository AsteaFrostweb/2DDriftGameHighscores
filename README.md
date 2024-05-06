# Drift Game Highscores
This is a simple ASP.NET Core 8  MVC Applicaiton which I made to handle the highscores of the "Drift Game"'s.
- https://github.com/AsteaFrostweb/3DDriftGame
- https://github.com/AsteaFrostweb/2DDriftGame

## Steps To Run:
  - Open project in Visual Studio
  - If running local databse then no need to change any config. Program will launch on localhost:433
  - If running on external database will need to re-config the setting in "appsettings.json"
  - If running on external machine, not "localhost", then you will need to re-config "https" settings in "LaunchSettings.json"

## Dependencies:
  - Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore (Version 8.0.3)
  - Microsoft.AspNetCore.Identity.EntityFrameworkCore (Version 8.0.3)
  - Microsoft.AspNetCore.Identity.UI (Version 8.0.3)
  - Microsoft.EntityFrameworkCore.Sqlite (Version 8.0.3)
  - Microsoft.EntityFrameworkCore.SqlServer (Version 8.0.3)
  - Microsoft.EntityFrameworkCore.Tools (Version 8.0.3)
  - Microsoft.VisualStudio.Web.CodeGeneration.Design (Version 8.0.2)

## Frameworks:
  - ASP.NET Core 8.0 
