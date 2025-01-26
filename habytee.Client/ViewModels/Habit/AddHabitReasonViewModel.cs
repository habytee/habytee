using System.ComponentModel.DataAnnotations;

namespace habytee.Client.ViewModels;

public class AddHabitReasonViewModel : BaseViewModel
{
    private string reason = string.Empty;

    [StringLength(3000, ErrorMessage = "The {0} can be at max {1} characters long", MinimumLength = 0)]
    public string Reason
    {
        get => reason;
        set
        {
            reason = value;
            OnPropertyChanged();
        }
    }

    public AddHabitReasonViewModel(BaseViewModel parentViewModel)
    {
        ParentViewModel = parentViewModel;
    }
}
