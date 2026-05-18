using Microsoft.AspNetCore.Mvc;

namespace CreativeLab.WebApi.Controllers;

public class UploadController : BaseController
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
    private static readonly string[] AllowedVideoExtensions = [".mp4", ".webm", ".mov", ".avi"];
    private const long MaxImageFileSize = 5 * 1024 * 1024; // 5MB
    private const long MaxVideoFileSize = 1000 * 1024 * 1024; // 100MB

    [HttpPost]
    public async Task<ActionResult<UploadImagesResult>> Images(
        [FromForm] IFormFileCollection files,
        CancellationToken cancellationToken)
    {
        if (files == null || files.Count == 0)
            return BadRequest("No files provided");

        if (files.Count > 10)
            return BadRequest("Maximum 10 files allowed");

        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        Directory.CreateDirectory(uploadsDir);

        var urls = new List<string>();

        foreach (var file in files)
        {
            if (file.Length == 0) continue;
            if (file.Length > MaxImageFileSize)
                return BadRequest($"File '{file.FileName}' exceeds 5MB limit");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedImageExtensions.Contains(ext))
                return BadRequest($"File type '{ext}' is not allowed");

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            urls.Add($"/uploads/{fileName}");
        }

        return Ok(new UploadImagesResult { Urls = urls });
    }

    [HttpPost]
    public async Task<ActionResult<UploadVideoResult>> Video(
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided");

        if (file.Length > MaxVideoFileSize)
            return BadRequest($"File '{file.FileName}' exceeds 100MB limit");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedVideoExtensions.Contains(ext))
            return BadRequest($"Video type '{ext}' is not allowed. Allowed: .mp4, .webm, .mov, .avi");

        var videosDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");
        Directory.CreateDirectory(videosDir);

        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(videosDir, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return Ok(new UploadVideoResult { Url = $"/videos/{fileName}" });
    }
}

public class UploadImagesResult
{
    public List<string> Urls { get; set; } = [];
}

public class UploadVideoResult
{
    public string Url { get; set; } = "";
}
