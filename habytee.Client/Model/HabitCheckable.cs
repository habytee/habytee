using habytee.Interconnection.Models;

namespace habytee.Client.Model;

public class HabitCheckable
{
    public Habit Habit { get; set; } = null!;
    public bool IsCompleted { get; set; } 
}
