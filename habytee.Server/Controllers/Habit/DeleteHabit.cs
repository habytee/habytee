using habytee.Server.Core;
using habytee.Server.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace habytee.Server.Controllers
{
	public partial class HabitController : BaseController
	{
		[HttpDelete("{id}")]
        [ServiceFilter(typeof(HabitBelongsToUserFilter))]
		public IActionResult DeleteHabit(int id)
		{
			WriteDbContext.Habits.Remove(CurrentHabit!);
			WriteDbContext.SaveChanges();

			return Ok(new { message = "Habit deleted successfully" });
		}
	}
}
