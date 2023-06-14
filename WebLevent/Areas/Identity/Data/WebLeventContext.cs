using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebLevent.Areas.Identity.Data;
using WebLevent.Models;

namespace WebLevent.Areas.Identity.Data;

public class WebLeventContext : IdentityDbContext<WebLeventUser>
{
    public WebLeventContext(DbContextOptions<WebLeventContext> options)
        : base(options)
    {
    }

    protected WebLeventContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new LeventCoreUserEntityConfiguration());
    }
    internal class LeventCoreUserEntityConfiguration : IEntityTypeConfiguration<WebLeventUser>
    {
        public void Configure(EntityTypeBuilder<WebLeventUser> builder)
        {
            builder.Property(u => u.FullName).HasMaxLength(255);
        }
    }
    public DbSet<WebLevent.Models.SanPham>? SanPham { get; set; }
    public DbSet<WebLevent.Models.LoaiSanPham>? LoaiSanPham { get; set; }
}
