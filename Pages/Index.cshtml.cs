using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Water_Logger.Models;

namespace Water_Logger.Pages;

public class IndexModel : PageModel
{
    private IConfiguration _config;
    [BindProperty]
    public List<DrinkingWater> DrinkWaterLogs { get; set;} = new();
    public IndexModel(IConfiguration config)
    {
        _config = config;
    }

    public void OnGet()
    {
        using SqliteConnection connection = new SqliteConnection(_config.GetConnectionString("SQLiteConnection"));
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        string sqlQuery = @"SELECT * FROM drinking_water;";

        command.CommandText = sqlQuery;

        SqliteDataReader reader = command.ExecuteReader();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                DrinkWaterLogs.Add(new DrinkingWater()
                {
                    Id = reader.GetInt32(0),
                   Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", CultureInfo.InvariantCulture),
                   Quantity = reader.GetInt32(2)

                });
            }
        }

        connection.Close();
    }
}
