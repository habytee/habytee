using System.ComponentModel.DataAnnotations;

namespace habytee.Client.ViewModels;

public class AddHabitNameViewModel : BaseViewModel
{
    private string name = string.Empty;

    [Required]
    [StringLength(15, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 4)]
    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged();
            ((ParentViewModel as AddHabitViewModel)?.Next as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }

    public AddHabitNameViewModel(BaseViewModel parentViewModel)
    {
        ParentViewModel = parentViewModel;
    }
}
