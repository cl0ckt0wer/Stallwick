using System.ComponentModel.DataAnnotations;

namespace Stallwick.Data;

public class Listing
{
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Range(0, 1_000_000)]
    public decimal Price { get; set; }

    [Url]
    [StringLength(2000)]
    public string? ImageUrl { get; set; }

    [Required]
    [StringLength(120)]
    public string LocationName { get; set; } = string.Empty;

    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }

    /// <summary>UTC timestamp; SQLite cannot sort by DateTimeOffset.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string SellerId { get; set; } = string.Empty;

    public ApplicationUser? Seller { get; set; }
}
