using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Training_and_Workout_App.API.Controllers;

[ApiController]
[Route("api/media")]
[Authorize]
public class MediaController(IWebHostEnvironment environment) : AppControllerBase
{
    private static readonly HashSet<string> AllowedFolders = new(StringComparer.OrdinalIgnoreCase)
    {
        "plan-customizations",
        "profiles"
    };

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp",
        ".gif"
    };

    [HttpPost("images")]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromQuery] string folder)
    {
        if (file.Length == 0)
        {
            return BadRequest(new { message = "File is empty." });
        }

        if (!AllowedFolders.Contains(folder))
        {
            return BadRequest(new { message = "Invalid media folder." });
        }

        var extension = Path.GetExtension(file.FileName);
        if (!AllowedExtensions.Contains(extension))
        {
            return BadRequest(new { message = "Unsupported image format." });
        }

        var fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var relativeFolder = Path.Combine("uploads", folder);
        var absoluteFolder = Path.Combine(environment.WebRootPath, relativeFolder);
        Directory.CreateDirectory(absoluteFolder);

        var absolutePath = Path.Combine(absoluteFolder, fileName);
        await using (var stream = System.IO.File.Create(absolutePath))
        {
            await file.CopyToAsync(stream);
        }

        var imageUrl = $"/uploads/{folder}/{fileName}";
        return Ok(new { imageUrl, fileName });
    }
}
