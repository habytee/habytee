using Microsoft.AspNetCore.Mvc;
using habytee.Interconnection.Models;
using habytee.Server.Core;
using Habytee.Interconnection.Dto;

namespace habytee.Server.Controllers
{
	public partial class HabitController : BaseController
	{
		[HttpPost]
		public IActionResult CreateHabit([FromBody] CreateHabitDto habitDto)
		{
			if(CurrentUser.Habits.Count >= 300)
			{
				return BadRequest("User already has 300 habits");
			}

            if(habitDto.Alarm != null)
            {
                habitDto.Alarm = DateTime.SpecifyKind(habitDto.Alarm.Value, DateTimeKind.Utc);
            }

			var newHabit = new Habit
			{
				Name = habitDto.Name,
				Reason = habitDto.Reason,
				ABBoth = habitDto.ABBoth,
				AWeekDays = habitDto.AWeekDays,
				BWeekDays = habitDto.BWeekDays,
				Alarm = habitDto.Alarm,
				Earnings = habitDto.Earnings,
				UserId = CurrentUser.Id
			};

			WriteDbContext.Habits.Add(newHabit);
			WriteDbContext.SaveChanges();

			return Ok(newHabit);
		}
	}
}
