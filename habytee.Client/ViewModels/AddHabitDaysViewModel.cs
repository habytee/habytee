using System;
using habytee.Interconnection.Models;
using habytee.Interconnection.Validator;

namespace habytee.Client.ViewModels;

public class AddHabitDaysViewModel : BaseViewModel
{
    private bool aBActivated;
    public bool ABActivated
    {
        get => aBActivated;
        set
        {
            aBActivated = value;
            OnPropertyChanged();
        }
    }
    
    private IEnumerable<DayOfWeek> aWeek = new List<DayOfWeek>();
    public IEnumerable<DayOfWeek> AWeek
    {
        get => aWeek;
        set
        {
            aWeek = value;
            OnPropertyChanged();
        }
    }

    private IEnumerable<DayOfWeek> bWeek = new List<DayOfWeek>();
    public IEnumerable<DayOfWeek> BWeek
    {
        get => bWeek;
        set
        {
            bWeek = value;
            OnPropertyChanged();
        }
    }

    public AddHabitDaysViewModel(BaseViewModel parentViewModel)
    {
        ParentViewModel = parentViewModel;
    }

    public override bool IsValid
    {
        get => Interconnection.Validator.Validate.HabitAB(ABActivated, AWeek, BWeek);
    }
}
