using Microsoft.AspNetCore.Mvc;
using habytee.Interconnection.Models;
using habytee.Server.Core;

namespace habytee.Server.Controllers
{
	public partial class HabitController : BaseController
	{
		[HttpPost]
		public IActionResult CreateHabit([FromBody] Habit habit)
		{
			if(CurrentUser.Habits.Count >= 300)
			{
				return BadRequest("User already has 300 habits");
			}

            if(habit.Alarm != null)
            {
                habit.Alarm = DateTime.SpecifyKind(habit.Alarm.Value, DateTimeKind.Utc);
            }

			var newHabit = new Habit
			{
				Name = habit.Name,
				Reason = habit.Reason,
				ABBoth = habit.ABBoth,
				AWeekDays = habit.AWeekDays,
				BWeekDays = habit.BWeekDays,
				Alarm = habit.Alarm,
				Earnings = habit.Earnings,
				UserId = CurrentUser.Id
			};

			WriteDbContext.Habits.Add(newHabit);
			WriteDbContext.SaveChanges();

			return Ok(new { message = "Habit created successfully", id = newHabit.Id });
		}
	}
}
