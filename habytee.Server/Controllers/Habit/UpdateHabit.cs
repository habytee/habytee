using Microsoft.AspNetCore.Mvc;
using habytee.Server.Core;
using habytee.Interconnection.Models;
using habytee.Server.Middleware;

namespace habytee.Server.Controllers
{
	public partial class HabitController : BaseController
	{
		[HttpPut("{id}")]
		[ServiceFilter(typeof(HabitBelongsToUserFilter))]
		public IActionResult UpdateHabit(int id, [FromBody] Habit habit)
		{
			CurrentHabit!.Name = habit.Name;
			CurrentHabit!.Reason = habit.Reason;
			CurrentHabit!.ABBoth = habit.ABBoth;
			CurrentHabit!.AWeekDays = habit.AWeekDays;
			CurrentHabit!.BWeekDays = habit.BWeekDays;
			CurrentHabit!.Alarm = habit.Alarm;
			CurrentHabit!.Earnings = habit.Earnings;
			WriteDbContext.SaveChanges();

			return Ok(new { message = "Habit updated successfully" });
		}
	}
}
