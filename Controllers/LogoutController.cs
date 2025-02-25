using Microsoft.AspNetCore.Mvc;

namespace RedirectManager.Controllers;

public class LogoutController : ControllerBase
{
	[HttpGet("/admin/logout")]
	public IActionResult Logout()
	{
		Response.Cookies.Delete(Constants.CookieName);
		return Redirect("/admin/login");
	}
}
