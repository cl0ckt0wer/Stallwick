using Stallwick.Services;

namespace Stallwick.Tests;

public class GeoDistanceTests
{
    [Fact]
    public void SamePointIsZeroKm()
    {
        Assert.Equal(0, GeoDistance.HaversineKm(47.6062, -122.3321, 47.6062, -122.3321), 6);
    }

    [Fact]
    public void SeattleToPortlandIsAboutTwoHundredThirtyKm()
    {
        var km = GeoDistance.HaversineKm(47.6062, -122.3321, 45.5152, -122.6784);
        Assert.InRange(km, 230, 236);
    }

    [Fact]
    public void DistanceIsSymmetric()
    {
        var ab = GeoDistance.HaversineKm(51.5074, -0.1278, 48.8566, 2.3522);
        var ba = GeoDistance.HaversineKm(48.8566, 2.3522, 51.5074, -0.1278);
        Assert.Equal(ab, ba, 9);
    }

    [Fact]
    public void CloserPointSortsFirst()
    {
        var origin = (Lat: 47.6062, Lon: -122.3321);
        var bellevue = GeoDistance.HaversineKm(origin.Lat, origin.Lon, 47.6101, -122.2015);
        var portland = GeoDistance.HaversineKm(origin.Lat, origin.Lon, 45.5152, -122.6784);
        Assert.True(bellevue < portland);
    }
}
