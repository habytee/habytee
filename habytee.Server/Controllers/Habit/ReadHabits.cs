using habytee.Server.Core;
using habytee.Server.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace habytee.Server.Controllers
{
	public partial class HabitController : BaseController
	{
		[HttpGet]
		public IActionResult GetAllHabits()
		{
			var habits = ReadDbContext.Habits.Where(h => h.UserId == CurrentUser.Id).ToList();

			return Ok(habits);
		}
	}
}
