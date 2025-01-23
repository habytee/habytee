namespace habytee.Interconnection.Models.Requests;

public class CreateHabitRequest()
{
    public string Name { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public bool ABBoth { get; set; }
    public List<DayOfWeek> AWeekDays { get; set; } = [];
    public List<DayOfWeek> BWeekDays { get; set; } = [];
    public DateTime? Alarm { get; set;}
    public int Earnings { get; set; }
	public DateTime? CreationDate { get; set; }
}
