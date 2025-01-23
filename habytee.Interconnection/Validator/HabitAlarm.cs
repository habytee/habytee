namespace habytee.Interconnection.Validator;

public static partial class Validate
{
    public static bool HabitAlarm(bool alarmActivated, DateTime? alarmTime)
    {
        if(alarmActivated && alarmTime == null)
        {
            return false;
        }

        return true;
    }
}
