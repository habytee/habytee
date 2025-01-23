using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace habytee.Interconnection.Models;

public class Habit
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Reason { get; set; } = string.Empty;

    [Required]
    public bool ABBoth { get; set; }

    [Required]
    public List<DayOfWeek> AWeekDays { get; set; } = [];
    public List<DayOfWeek> BWeekDays { get; set; } = [];
    public DateTime? Alarm { get; set; }

    [Required]
    public int Earnings { get; set; }
	[Required]
	public DateTime? CreationDate { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public int UserId { get; set; } 
    
    [JsonIgnore]
    public User User { get; set; } = null!;
    
	public List<HabitCheckedEvent> HabitCheckedEvents { get; set; } = [];

	public Habit()
    {

    }
}
