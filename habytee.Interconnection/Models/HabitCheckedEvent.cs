using System.Text.Json.Serialization;

namespace habytee.Interconnection.Models;
public class HabitCheckedEvent
{
	public int Id { get; set; }

	[JsonIgnore]
	public int HabitId { get; set; }

	[JsonIgnore]
	public Habit Habit { get; set; }  = null!;
	
	public DateTime? TimeStamp { get; set; }

	public HabitCheckedEvent()
	{

	}
}
