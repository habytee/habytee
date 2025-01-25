using Microsoft.AspNetCore.Mvc;
using habytee.Interconnection.Models;
using habytee.Server.Core;
using habytee.Server.Middleware;

namespace habytee.Server.Controllers
{
	public partial class HabitController : BaseController
	{
        // POST: api/habit/{habitId}/checkedEvent
        [HttpPost("{habitId}/checkedEvent")]
        [ServiceFilter(typeof(HabitBelongsToUserFilter))]
		public IActionResult CreateHabitCheckedEvent(int habitId, HabitCheckedEvent habitCheckedEvent)
		{
            if(CurrentHabit!.HabitCheckedEvents.Count >= 29)
            {
                WriteDbContext.Habits.Remove(CurrentHabit);
                WriteDbContext.SaveChanges();

                return Ok(new { message = "You learned a new habit!" });
            }

            CurrentHabit!.HabitCheckedEvents.Add(new HabitCheckedEvent{
                TimeStamp = DateTime.UtcNow
            });
			WriteDbContext.SaveChanges();

			return Ok(new { message = "Habit checked event created successfully" });
		}
	}
}
