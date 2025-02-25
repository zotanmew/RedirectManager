using Microsoft.EntityFrameworkCore;
using RedirectManager.Database.Tables;

namespace RedirectManager.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
	public DbSet<Redirect> Redirects { get; init; } = null!;
}
