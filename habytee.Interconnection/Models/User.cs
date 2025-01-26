using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace habytee.Interconnection.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Email { get; set; } = string.Empty;

    public ObservableCollection<Habit> Habits { get; set; } = [];

    public int Coins { get; set; }

    public bool? LightTheme { get; set; }

    public CultureInfo? Culture { get; set; }

    public RegionInfo? Region { get; set; }

    public User()
    {
        
    }
}
