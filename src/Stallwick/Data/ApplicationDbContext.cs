using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Stallwick.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Listing> Listings => Set<Listing>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Listing>(listing =>
        {
            listing.HasIndex(l => l.CreatedAt);
            listing.Property(l => l.Price).HasColumnType("decimal(18,2)");
            listing.HasOne(l => l.Seller)
                .WithMany()
                .HasForeignKey(l => l.SellerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
