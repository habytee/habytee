using System.ComponentModel.DataAnnotations;
using habytee.Interconnection.Attributes;
using habytee.Interconnection.Models;

namespace Habytee.Interconnection.Dto;

public class CreateHabitDto
{
    [Required]
    public string Name { get; set; } = null!;

    public string Reason { get; set; } = null!;

    [Required]
    public bool ABBoth { get; set; }

    [Required]
    public List<DayOfWeek> AWeekDays { get; set; } = null!;

    [RequiredIf("ABBoth", true)]
    public List<DayOfWeek> BWeekDays { get; set; } = null!;
    
    public DateTime? Alarm { get; set; }

    [Required]
    public int Earnings { get; set; }

    public static CreateHabitDto CreateCreateHabitDto(Habit habit)
    {
        return new CreateHabitDto
        {
            Name = habit.Name,
            Reason = habit.Reason,
            ABBoth = habit.ABBoth,
            AWeekDays = habit.AWeekDays,
            BWeekDays = habit.BWeekDays,
            Alarm = habit.Alarm,
            Earnings = habit.Earnings
        };
    }
}
