using habytee.Server.Core;
using Microsoft.AspNetCore.Mvc;

namespace habytee.Server.Controllers
{
	public partial class UserController : BaseController
	{
		[HttpGet]
		public IActionResult GetUser()
		{
			return Ok(CurrentUser);
		}
	}
}
