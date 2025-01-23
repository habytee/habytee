namespace habytee.Interconnection.Models.Requests;
using System;

public class UpdateHabitRequest
{
	public string Name { get; set; } = null!;
	public string Reason { get; set; } = null!;
	public bool ABBoth { get; set; }
	public List<DayOfWeek> AWeekDays { get; set; } = null!;
	public List<DayOfWeek> BWeekDays { get; set; } = null!;
	public DateTime? Alarm{ get; set; }
	public int Earnings{ get; set; }
	public DateTime? CreationDate { get; set; }
}
