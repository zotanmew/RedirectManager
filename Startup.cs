using System.Threading.RateLimiting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using RedirectManager;
using RedirectManager.Components;
using RedirectManager.Database;
using RedirectManager.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents();
builder.Services.AddControllers();
builder.Services.AddOutputCache(opts => opts.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(15));
builder.Services.AddSqlite<DatabaseContext>("Data Source=database.db;");
builder.Services.AddSingleton<AuthorizationMiddleware>();
builder.Services.AddRateLimiter(options =>
{
	var authPolicy = new SlidingWindowRateLimiterOptions
	{
		PermitLimit          = 60,
		SegmentsPerWindow    = 60,
		Window               = TimeSpan.FromSeconds(60),
		QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
		QueueLimit           = 60
	};

	options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(AuthPolicyPartition);
	return;

	RateLimitPartition<string> AuthPolicyPartition(HttpContext ctx)
	{
		var key = ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault()
		          ?? ctx.Connection.RemoteIpAddress?.ToString() ?? "";

		return ctx.Request.Path.StartsWithSegments("/admin")
			? RateLimitPartition.GetSlidingWindowLimiter(key, _ => authPolicy)
			: RateLimitPartition.GetNoLimiter("nolimit");
	}
});

var app = builder.Build();

if (Constants.AuthToken == null!)
{
	app.Logger.LogCritical("Environment variable AUTH_TOKEN is not set, aborting startup");
	return;
}

await using (var scope = app.Services.CreateAsyncScope())
{
	var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
	await db.Database.MigrateAsync();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedProto });

if (!app.Environment.IsDevelopment())
	app.UseExceptionHandler("/error", createScopeForErrors: true);

app.UseAntiforgery();

app.UseOutputCache();
app.MapControllers();
app.MapStaticAssets();
app.UseRateLimiter();
app.UseMiddleware<AuthorizationMiddleware>();
app.UsePathBase("/admin");
app.MapRazorComponents<App>();

app.Run();
