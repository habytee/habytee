using habytee.Server.Core;
using habytee.Server.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace habytee.Server.Controllers
{
	public partial class HabitController : BaseController
	{
        // DELETE: api/habit/{habitId}/checkedEvent/{checkedEventId}
        [HttpDelete("{habitId}/checkedEvent/{checkedEventId}")]
        [ServiceFilter(typeof(HabitBelongsToUserFilter))]
		public IActionResult DeleteHabitCheckedEvent(int habitId, int checkedEventId)
		{
			var eventToRemove = CurrentHabit!.HabitCheckedEvents.FirstOrDefault(hce => hce.Id == checkedEventId);
			if (eventToRemove == null)
			{
                return NotFound("Habit checked event not found");
			}
			WriteDbContext.HabitCheckedEvents.Remove(eventToRemove);
			WriteDbContext.SaveChanges();

			return Ok(new { message = "Habit checked event deleted successfully" });
		}
	}
}
