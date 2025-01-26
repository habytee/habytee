using habytee.Server.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace habytee.Server.Controllers
{
	public partial class HabitController : BaseController
	{
		[HttpGet]
		public IActionResult GetAllHabits()
		{
			var habits = ReadDbContext.Habits.Where(h => h.UserId == CurrentUser.Id).Include(h => h.HabitCheckedEvents).ToList();

			return Ok(habits);
		}
	}
}
