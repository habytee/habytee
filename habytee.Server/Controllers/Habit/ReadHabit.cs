using habytee.Server.Core;
using habytee.Server.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace habytee.Server.Controllers
{
	public partial class HabitController : BaseController
	{
		[HttpGet("{id}")]
		[ServiceFilter(typeof(HabitBelongsToUserFilter))]
		public IActionResult GetHabit(int id)
		{
			return Ok(CurrentHabit);
		}
	}
}
