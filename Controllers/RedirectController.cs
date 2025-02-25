using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using RedirectManager.Database;

namespace RedirectManager.Controllers;

public class RedirectController(DatabaseContext db) : ControllerBase
{
	[OutputCache]
	[HttpGet("/")]
	public IActionResult Index() => NotFound();

	[OutputCache]
	[HttpGet("/{shortcode}")]
	public async Task<IActionResult> RedirectAsync(string shortcode)
	{
		var match = await db.Redirects.FirstOrDefaultAsync(p => p.Shortcode == shortcode);
		if (match == null) return NotFound();
		return RedirectPermanent(match.TargetUrl);
	}
}
