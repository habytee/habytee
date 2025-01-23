using System;

namespace habytee.Interconnection.Validator;

using habytee.Interconnection.Models;

public static partial class Validate
{
    public static bool HabitAB(bool abActivated, IEnumerable<DayOfWeek> weekA, IEnumerable<DayOfWeek> weekB)
    {
        bool aWeekSet = weekA.ToList<DayOfWeek>().Count > 0;
        bool bWeekSet = weekB.ToList<DayOfWeek>().Count > 0;

        if((aWeekSet || bWeekSet) && abActivated)
        {
            return true;
        }
        
        if(aWeekSet)
        {
            return true;
        }

        return false;
    }
}
