namespace habytee.Client.ViewModels;

public class AddHabitAlarmViewModel : BaseViewModel
{
    private bool setAlarm;
    public bool SetAlarm
    {
        get => setAlarm;
        set
        {
            setAlarm = value;
            OnPropertyChanged();
        }
    }

    private DateTime? alarmTime = null;
    public DateTime? AlarmTime
    {
        get => alarmTime;
        set
        {
            alarmTime = value;
            OnPropertyChanged();
        }
    }

    public AddHabitAlarmViewModel(BaseViewModel parentViewModel)
    {
        ParentViewModel = parentViewModel;
    }

    public override bool IsValid
    {
        get => Interconnection.Validator.Validate.HabitAlarm(SetAlarm, AlarmTime);
    }
}
