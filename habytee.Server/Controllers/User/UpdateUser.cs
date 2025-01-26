using Microsoft.AspNetCore.Mvc;
using habytee.Server.Core;
using Habytee.Interconnection.Dto;

namespace habytee.Server.Controllers
{
	public partial class UserController : BaseController
	{
		[HttpPut]
		public IActionResult UpdateUser([FromBody] UpdateUserDto user)
		{
			var dbUser = WriteDbContext.Users.Find(CurrentUser!.Id)!;
			dbUser.Coins = user.Coins;
			dbUser.LightTheme = user.LightTheme;
			dbUser.Language = user.Language;
			dbUser.Currency = user.Currency;
			WriteDbContext.SaveChanges();

			return Ok(dbUser);
		}
	}
}

