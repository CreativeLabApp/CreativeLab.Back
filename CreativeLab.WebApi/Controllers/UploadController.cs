using Microsoft.AspNetCore.Mvc;

namespace CreativeLab.WebApi.Controllers;

public class UploadController : BaseController
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
    private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

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
            if (file.Length > MaxFileSize)
                return BadRequest($"File '{file.FileName}' exceeds 5MB limit");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                return BadRequest($"File type '{ext}' is not allowed");

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            urls.Add($"/uploads/{fileName}");
        }

        return Ok(new UploadImagesResult { Urls = urls });
    }
}

public class UploadImagesResult
{
    public List<string> Urls { get; set; } = [];
}
