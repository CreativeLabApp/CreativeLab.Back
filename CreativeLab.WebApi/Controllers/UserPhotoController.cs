using CreativeLab.Application.Interfaces;
using CreativeLab.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserPhotoController : BaseController
{
    private readonly ICreativeLabDbContext _dbContext;
    private readonly IWebHostEnvironment _environment;
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
    private const long MaxFileSize = 5 * 1024 * 1024; // 5MB
    private const string UploadsFolder = "userphotos";

    public UserPhotoController(ICreativeLabDbContext dbContext, IWebHostEnvironment environment)
    {
        _dbContext = dbContext;
        _environment = environment;
    }

    /// <summary>
    /// Загрузить фото пользователя
    /// </summary>
    [HttpPost("upload")]
    public async Task<ActionResult<UserPhotoDto>> UploadPhoto(
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided");

        if (file.Length > MaxFileSize)
            return BadRequest("File size exceeds 5MB limit");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(ext))
            return BadRequest($"File type '{ext}' is not allowed");

        var userId = UserId;
        if (userId == null)
            return Unauthorized("User not authenticated");

        // Создаем директорию для фотографий пользователя
        var userUploadsDir = Path.Combine(_environment.WebRootPath, "uploads", UploadsFolder, userId.ToString()!);
        Directory.CreateDirectory(userUploadsDir);

        // Генерируем уникальное имя файла
        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(userUploadsDir, fileName);

        // Сохраняем файл
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        // Создаем запись в базе данных
        var userPhoto = new UserPhoto
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FileName = fileName,
            OriginalFileName = file.FileName,
            ContentType = file.ContentType,
            FileSize = file.Length,
            FilePath = $"/uploads/{UploadsFolder}/{userId}/{fileName}",
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.UserPhotos.AddAsync(userPhoto, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(new UserPhotoDto
        {
            Id = userPhoto.Id,
            FileName = userPhoto.FileName,
            FilePath = userPhoto.FilePath,
            OriginalFileName = userPhoto.OriginalFileName,
            ContentType = userPhoto.ContentType,
            FileSize = userPhoto.FileSize,
            CreatedAt = userPhoto.CreatedAt
        });
    }

    /// <summary>
    /// Получить все фото пользователя
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<List<UserPhotoDto>>> GetUserPhotos(Guid userId, CancellationToken cancellationToken)
    {
        var photos = await _dbContext.UserPhotos
            .Where(p => p.UserId == userId && !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);

        return Ok(photos.Select(p => new UserPhotoDto
        {
            Id = p.Id,
            FileName = p.FileName,
            FilePath = p.FilePath,
            OriginalFileName = p.OriginalFileName,
            ContentType = p.ContentType,
            FileSize = p.FileSize,
            CreatedAt = p.CreatedAt
        }));
    }

    /// <summary>
    /// Получить мои фото
    /// </summary>
    [HttpGet("my")]
    public async Task<ActionResult<List<UserPhotoDto>>> GetMyPhotos(CancellationToken cancellationToken)
    {
        var userId = UserId;
        if (userId == null)
            return Unauthorized("User not authenticated");

        return await GetUserPhotos(userId, cancellationToken);
    }

    /// <summary>
    /// Удалить фото пользователя (мягкое удаление)
    /// </summary>
    [HttpDelete("{photoId:guid}")]
    public async Task<ActionResult> DeletePhoto(Guid photoId, CancellationToken cancellationToken)
    {
        var userId = UserId;
        if (userId == null)
            return Unauthorized("User not authenticated");

        var photo = await _dbContext.UserPhotos
            .FirstOrDefaultAsync(p => p.Id == photoId && !p.IsDeleted, cancellationToken);

        if (photo == null)
            return NotFound("Photo not found");

        if (photo.UserId != userId)
            return Forbid();

        // Мягкое удаление
        photo.IsDeleted = true;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Удалить фото пользователя (физическое удаление)
    /// </summary>
    [HttpDelete("{photoId:guid}/permanent")]
    public async Task<ActionResult> DeletePhotoPermanently(Guid photoId, CancellationToken cancellationToken)
    {
        var userId = UserId;
        if (userId == null)
            return Unauthorized("User not authenticated");

        var photo = await _dbContext.UserPhotos
            .FirstOrDefaultAsync(p => p.Id == photoId, cancellationToken);

        if (photo == null)
            return NotFound("Photo not found");

        if (photo.UserId != userId)
            return Forbid();

        // Удаляем файл
        var filePath = Path.Combine(_environment.WebRootPath, "uploads", UploadsFolder, userId.ToString()!, photo.FileName);
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        // Удаляем запись из базы данных
        _dbContext.UserPhotos.Remove(photo);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}

public class UserPhotoDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime CreatedAt { get; set; }
}