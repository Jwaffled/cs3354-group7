using Microsoft.EntityFrameworkCore;
using TextbookExchangeApp.EntityFramework;
using TextbookExchangeApp.Enums;
using TextbookExchangeApp.Services.Listing.Dto;

namespace TextbookExchangeApp.Services.Listing;

public class ListingService : IListingService
{
    private readonly ApplicationDbContext _dbContext;

    public ListingService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<int> CreateListingAsync(CreateListingDto dto)
    {
        var listing = new Models.Listing
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            Condition = dto.Condition,
            ImageUrl = dto.ImageUrl
        };
        
        _dbContext.Listings.Add(listing);
        
        await _dbContext.SaveChangesAsync();

        return listing.Id;
    }

    public async Task<ListingListItemDto?> GetListingByIdAsync(int id)
    {
        var data = await _dbContext.Listings.FirstOrDefaultAsync(x => x.Id == id);

        return data == null ? null : new ListingListItemDto
        {
            Id = data.Id,
            Condition = data.Condition.GetDisplayName(),
            Description = data.Description,
            ImageUrl = data.ImageUrl,
            Price = data.Price,
            Title = data.Title,
        };
    }

    public async Task<ListingListItemDto?> GetListingDetailsAsync(int id)
    {
        var data = await _dbContext.Listings
            .AsNoTracking()
            .Include(x => x.CreatedBy)
            .FirstOrDefaultAsync(x => x.Id == id);
        return data == null
            ? null
            : new ListingListItemDto
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                Condition = data.Condition.GetDisplayName(),
                AuthorName = data.CreatedBy.FirstName + " " + data.CreatedBy.LastName,
                CreatedAt = data.CreatedAt,
                CreatedById = data.CreatedById,
                Price = data.Price,
                ImageUrl = data.ImageUrl,
            };
    }

    public async Task<List<ListingListItemDto>> GetAllListingsAsync()
    {
        var data = await _dbContext.Listings
            .AsNoTracking()
            .Include(x => x.CreatedBy)
            .Select(x => new ListingListItemDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Condition = x.Condition.GetDisplayName(),
                AuthorName = x.CreatedBy.FirstName + " " + x.CreatedBy.LastName,
                CreatedAt = x.CreatedAt,
                Price = x.Price,
                ImageUrl = x.ImageUrl,
            })
            .ToListAsync();

        return data;
    }
}