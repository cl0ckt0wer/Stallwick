# Stallwick

A marketplace where people post their stuff for sale, browse it as a grid, and see what's listed near them ordered by distance.

Built with Blazor Web App (.NET 10), ASP.NET Core Identity, EF Core, and SQLite.

## Features

- **Post a listing** (`/sell`, sign-in required): title, description, price, image URL, location name, and coordinates. Coordinates can be filled from the browser's geolocation or typed in manually.
- **Browse grid** (`/browse`): every listing, newest first.
- **Nearby grid** (`/nearby`): listings ordered by great-circle distance from your location, with the distance shown on each card.
- **Home** (`/`): the latest listings plus links into the rest of the app.

Distances use the Haversine formula (`Services/GeoDistance.cs`). SQLite can't translate the trig, so nearby ordering happens in memory after the rows are loaded.

## Running locally

Requires the .NET 10 SDK.

```bash
dotnet run --project src/Stallwick
```

The app applies EF Core migrations on startup and creates `src/Stallwick/Data/app.db` (gitignored). Register an account to post — email confirmation is disabled because no email sender is configured yet.

## Container

Images are published to GHCR on every push to `main` and on `v*` tags:

```bash
docker run -p 8080:8080 -v stallwick-data:/app/data ghcr.io/cl0ckt0wer/stallwick:latest
```

The app listens on port 8080 and keeps its SQLite database in `/app/data`, so mount a volume there to persist listings. Build locally with `docker build -t stallwick .`.

## Tests

```bash
dotnet test
```

## Project layout

```
src/Stallwick/        Blazor Web App
  Components/Pages/   Home, Browse, Nearby, Sell
  Components/Shared/  ListingCard
  Data/               Listing, ApplicationDbContext, migrations
  Services/           ListingService, GeoDistance
tests/Stallwick.Tests/
```
