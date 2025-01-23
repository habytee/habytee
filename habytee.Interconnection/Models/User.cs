using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace habytee.Interconnection.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Email { get; set; } = string.Empty;

    public List<Habit> Habits { get; set; } = [];

    public User()
    {
        
    }
}
