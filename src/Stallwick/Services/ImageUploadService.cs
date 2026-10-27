using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Forms;

namespace Stallwick.Services;

public class ImageUploadService(IWebHostEnvironment environment, IConfiguration configuration)
{
    public const long MaxFileSize = 5 * 1024 * 1024;

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".gif"
    };

    private readonly string _uploadsPath = ResolveUploadsPath(environment, configuration);

    public string UploadsPath => _uploadsPath;

    public async Task<string> SaveAsync(IBrowserFile file, CancellationToken cancellationToken = default)
    {
        if (file.Size > MaxFileSize)
        {
            throw new InvalidOperationException("Photo must be smaller than 5 MB.");
        }

        var extension = Path.GetExtension(file.Name);
        if (!AllowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException("Only .jpg, .jpeg, .png, .webp, and .gif images are allowed.");
        }

        Directory.CreateDirectory(_uploadsPath);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.GetFullPath(Path.Combine(_uploadsPath, fileName));

        if (!IsWithinUploads(filePath))
        {
            throw new InvalidOperationException("Invalid upload path.");
        }

        await using var source = file.OpenReadStream(MaxFileSize, cancellationToken);
        await using var destination = File.Create(filePath);
        await source.CopyToAsync(destination, cancellationToken);

        return $"/uploads/{fileName}";
    }

    public string? GetContentType(string fileName)
    {
        return Path.GetExtension(fileName).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            _ => null
        };
    }

    public bool TryGetFilePath(string fileName, [NotNullWhen(true)] out string? filePath)
    {
        filePath = Path.GetFullPath(Path.Combine(_uploadsPath, fileName));
        if (!IsWithinUploads(filePath) || !File.Exists(filePath))
        {
            filePath = null;
            return false;
        }

        return true;
    }

    private bool IsWithinUploads(string filePath)
    {
        var fullFile = Path.GetFullPath(filePath);
        var fullUploads = Path.GetFullPath(_uploadsPath);
        return fullFile.StartsWith(fullUploads + Path.DirectorySeparatorChar, StringComparison.Ordinal)
            || fullFile.Equals(fullUploads, StringComparison.Ordinal);
    }

    private static string ResolveUploadsPath(IWebHostEnvironment environment, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        string dataSource;
        try
        {
            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };
            dataSource = (builder.TryGetValue("DataSource", out var value) ? value : null)?.ToString()
                ?? "app.db";
        }
        catch
        {
            dataSource = "app.db";
        }

        var dbDirectory = Path.GetDirectoryName(Path.GetFullPath(dataSource, environment.ContentRootPath));
        return Path.Combine(dbDirectory ?? environment.ContentRootPath, "uploads");
    }
}
