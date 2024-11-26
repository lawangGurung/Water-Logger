using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Water_Logger.Models;

namespace Water_Logger.Pages
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public required DrinkingWater LoggingData { get; set; } = new();
        private IConfiguration _config;
        public EditModel(IConfiguration config)
        {
            _config = config;
        }
        public void OnGet(int id)
        {
            using SqliteConnection connection = new SqliteConnection(_config.GetConnectionString("SQLiteConnection"));
            connection.Open();
            string sqlQuery = @$"SELECT * FROM drinking_water
                WHERE Id = {id}";
            var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = sqlQuery;
            var reader = sqlCommand.ExecuteReader();
            
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    LoggingData.Id = reader.GetInt32(0);
                    LoggingData.Date = DateTime.ParseExact(reader.GetString(1),"dd-MM-yyyy", CultureInfo.InvariantCulture);
                    LoggingData.Quantity = reader.GetInt32(2);
                }
            }

            connection.Close();
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            using SqliteConnection connection = new SqliteConnection(_config.GetConnectionString("SQLiteConnection"));
            connection.Open();
            var sqlQuery = @$"UPDATE drinking_water
                SET Date = '{LoggingData.Date.ToString("dd-MM-yyyy")}',
                    Quantity = {LoggingData.Quantity}
                WHERE Id = {LoggingData.Id}";
            
            var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = sqlQuery;

            sqlCommand.ExecuteNonQuery();
            connection.Close();

            TempData["success"] = "Log Edited Sucessfully!!";
            return RedirectToPage("/Index");
        }
    }
}
