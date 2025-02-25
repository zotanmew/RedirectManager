namespace RedirectManager.Middleware;

public class AuthorizationMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
	{
		if (ctx.Request.Path == "/admin/login" || !ctx.Request.Path.StartsWithSegments("/admin"))
		{
			await next(ctx);
			return;
		}

		if (ctx.Request.Cookies.TryGetValue(Constants.CookieName, out var token) && token == Constants.AuthToken)
		{
			ctx.Items["authenticated"] = true;
			await next(ctx);
			return;
		}

		ctx.Response.Redirect("/admin/login");
	}
}
