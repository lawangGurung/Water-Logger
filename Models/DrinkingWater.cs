using System.ComponentModel.DataAnnotations;

namespace Water_Logger.Models;

public class DrinkingWater
{
    [Key]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}
