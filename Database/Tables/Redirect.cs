using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RedirectManager.Database.Tables;

public class Redirect
{
	public Guid     Id        { get; set; }
	public string   Shortcode { get; set; } = null!;
	public DateTime DateAdded { get; set; }
	public string   TargetUrl { get; set; } = null!;

	private class RedirectEntityConfiguration : IEntityTypeConfiguration<Redirect>
	{
		public void Configure(EntityTypeBuilder<Redirect> builder)
		{
			builder.HasKey(p => p.Id);
			builder.HasIndex(p => p.Shortcode).IsUnique();
			builder.Property(p => p.DateAdded).HasDefaultValueSql("now()");
		}
	}
}
