using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedirectManager.Database;

namespace RedirectManager.Controllers;

public class ExportController(DatabaseContext db) : ControllerBase
{
	[HttpGet("/admin/export")]
	[Produces("application/json")]
	public async Task<IActionResult> ExportAsync()
	{
		var data  = await db.Redirects.ToListAsync();
		var bytes = JsonSerializer.SerializeToUtf8Bytes(data);
		return File(bytes, "application/json", "redirects.json");
	}
}
