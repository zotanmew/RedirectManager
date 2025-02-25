using System.Collections.Immutable;

namespace RedirectManager;

public class Constants
{
	public static readonly ImmutableHashSet<string> SystemUrls = ["admin", "error"];
	public static readonly string                   AuthToken  = Environment.GetEnvironmentVariable("AUTH_TOKEN")!;
	public const           string                   CookieName = "token";
}
