using Eskon_APIs.Contracts.MediaItem;
using Eskon_APIs.Errors;

namespace Eskon_APIs.Services;

public class MediaService : IMediaService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostingEnvironment;

    public MediaService(ApplicationDbContext context, IWebHostEnvironment webHostingEnvironment)
    {
        _context = context;
        _webHostingEnvironment = webHostingEnvironment;
    }

    public async Task<Result> DeleteImageAsync(int houseId, string ownerId, int mediaItemId, CancellationToken cancellationToken = default)
    {
        var mediaItem = await _context.MediaItem
        .Include(m => m.House)
        .FirstOrDefaultAsync(m => m.MediaId == mediaItemId && m.HouseId == houseId, cancellationToken);

        if (mediaItem is null)
        {
            return Result.Failure(MediaItemErrors.NotFound);
        }

        if (mediaItem.House.OwnerId != ownerId)
        {
            return Result.Failure(HouseErrors.NotOwner);
        }

        var physicalPath = Path.Combine(_webHostingEnvironment.WebRootPath, mediaItem.URL.TrimStart('/'));
        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }

        _context.MediaItem.Remove(mediaItem);

        if (mediaItem.IsCover)
        {
            var newCoverImage = await _context.MediaItem
                .Where(m => m.HouseId == houseId && m.MediaId != mediaItemId)
                .OrderBy(m => m.SortOrder)
                .FirstOrDefaultAsync(cancellationToken);

            if (newCoverImage is not null)
            {
                newCoverImage.IsCover = true;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> SetCoverImageAsync(int houseId, string ownerId, int mediaItemId, CancellationToken cancellationToken = default)
    {
        var house = await _context.House
        .Include(h => h.MediaItems)
        .FirstOrDefaultAsync(h => h.HouseId == houseId, cancellationToken);

        if (house is null)
        {
            return Result.Failure(HouseErrors.HouseNotFound);
        }

        if (house.OwnerId != ownerId)
        {
            return Result.Failure(HouseErrors.NotOwner);
        }

        var newCover = house.MediaItems.FirstOrDefault(m => m.MediaId == mediaItemId);
        if (newCover is null)
        {
            return Result.Failure(MediaItemErrors.NotFound);
        }

        foreach (var image in house.MediaItems)
        {
            image.IsCover = (image.MediaId == mediaItemId);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<MediaItemResponse>> UploadImageForHouseAsync(int houseId, string ownerId, IFormFile imageFile, CancellationToken cancellationToken = default)
    {
        var house = await _context.House
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.HouseId == houseId, cancellationToken);

        if (house is null)
        {
            return Result.Failure<MediaItemResponse>(HouseErrors.HouseNotFound);
        }
        if (house.OwnerId != ownerId)
        {
            return Result.Failure<MediaItemResponse>(HouseErrors.NotOwner);
        }

        if (imageFile == null || imageFile.Length == 0)
        {
            return Result.Failure<MediaItemResponse>(MediaItemErrors.NoFile);
        }
        if (imageFile.Length > 5 * 1024 * 1024)
        {
            return Result.Failure<MediaItemResponse>(MediaItemErrors.FileTooLarge);
        }

        var fileExtension = Path.GetExtension(imageFile.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

        var uploadsFolderPath = Path.Combine(_webHostingEnvironment.WebRootPath, "images");
        var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

        Directory.CreateDirectory(uploadsFolderPath);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream, cancellationToken);
        }

        var existingImages = _context.MediaItem.Where(m => m.HouseId == houseId);

        var isFirstImage = !await existingImages.AnyAsync(cancellationToken);

        var nextSortOrder = await existingImages.AnyAsync(cancellationToken)
            ? await existingImages.MaxAsync(m => m.SortOrder, cancellationToken) + 1
            : 0;

        var mediaItem = new MediaItem
        {
            HouseId = houseId,
            URL = $"/images/{uniqueFileName}",
            IsCover = isFirstImage,
            SortOrder = nextSortOrder
        };

        await _context.MediaItem.AddAsync(mediaItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = mediaItem.Adapt<MediaItemResponse>();

        var baseUrl = "http://localhost:8088";

        if (!string.IsNullOrEmpty(response.URL) && response.URL.StartsWith("/"))
        {
            response.URL = $"{baseUrl}{response.URL}";
        }

        return Result.Success(response);
    }
}
