using Microsoft.EntityFrameworkCore;
using Stallwick.Data;

namespace Stallwick.Services;

public record NearbyListing(Listing Listing, double DistanceKm);

public class ListingService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    public async Task<List<Listing>> GetRecentAsync(int take = 60, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Listings
            .AsNoTracking()
            .OrderByDescending(l => l.CreatedAt)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns listings ordered by great-circle distance from the given point.
    /// Distance is computed in memory because SQLite cannot translate trigonometry.
    /// </summary>
    public async Task<List<NearbyListing>> GetNearbyAsync(
        double latitude,
        double longitude,
        int take = 60,
        CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var listings = await db.Listings.AsNoTracking().ToListAsync(cancellationToken);

        return listings
            .Select(l => new NearbyListing(l, GeoDistance.HaversineKm(latitude, longitude, l.Latitude, l.Longitude)))
            .OrderBy(l => l.DistanceKm)
            .ThenByDescending(l => l.Listing.CreatedAt)
            .Take(take)
            .ToList();
    }

    public async Task<List<Listing>> GetBySellerAsync(string sellerId, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Listings
            .AsNoTracking()
            .Where(l => l.SellerId == sellerId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Listing?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Listings.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<Listing> CreateAsync(Listing listing, CancellationToken cancellationToken = default)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        listing.CreatedAt = DateTime.UtcNow;
        db.Listings.Add(listing);
        await db.SaveChangesAsync(cancellationToken);
        return listing;
    }
}
