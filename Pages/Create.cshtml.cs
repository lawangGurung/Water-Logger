using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Water_Logger.Models;

namespace Water_Logger.Pages
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public required DrinkingWater LoggingData { get; set; }
        public IConfiguration _config { get; set; }

        public CreateModel(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page(); 
            }

            using var sqlConnection = new SqliteConnection(_config.GetConnectionString("SQLiteConnection"));
            sqlConnection.Open();
            SqliteCommand sqlCommand = sqlConnection.CreateCommand();
            string sqlQuery = @$"INSERT INTO drinking_water
                (Date, Quantity)
                VALUES ('{LoggingData.Date.ToString("dd-MM-yyyy")}', {LoggingData.Quantity})";

            sqlCommand.CommandText = sqlQuery;
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return RedirectToPage("/Index");
        }
    }
}
