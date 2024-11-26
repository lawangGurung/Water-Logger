using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Water_Logger.Data;
using Water_Logger.Models;

namespace Water_Logger.Pages
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public required DrinkingWater LoggingData { get; set; }
        
        private Database _db;
        public CreateModel(Database db)
        {
            _db = db;
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page(); 
            }

            _db.Create(LoggingData);

            TempData["success"] = "Log Created Successfully!!";

            return RedirectToPage("/Index");
        }
    }
}
