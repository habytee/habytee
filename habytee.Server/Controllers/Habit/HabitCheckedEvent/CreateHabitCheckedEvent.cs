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
		public IActionResult CreateHabitCheckedEvent(int habitId)
		{
            if(CurrentHabit!.HabitCheckedEvents.Any(hev => hev.TimeStamp.Date == DateTime.UtcNow.Date))
            {
                return BadRequest(new { message = "You already checked this habit today" });
            }

            if(CurrentHabit!.HabitCheckedEvents.Count >= 29)
            {
                WriteDbContext.Habits.Remove(CurrentHabit);
                WriteDbContext.SaveChanges();
                
                return Ok(new { message = "You learned a new habit!" });
            }

            var habitCheckedEvent = new HabitCheckedEvent
            {
                TimeStamp = DateTime.UtcNow,
                HabitId = habitId
            };
            WriteDbContext.HabitCheckedEvents.Add(habitCheckedEvent);
            WriteDbContext.SaveChanges();

            return Ok(habitCheckedEvent);
		}
	}
}
