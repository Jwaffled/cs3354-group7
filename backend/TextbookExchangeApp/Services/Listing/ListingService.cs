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
    
    public async Task CreateListingAsync(ListingDto dto)
    {
        var listing = dto.ConvertToModel();
        _dbContext.Listings.Add(listing);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ListingDto?> GetListingByIdAsync(int id)
    {
        var data = await _dbContext.Listings.FirstOrDefaultAsync(x => x.Id == id);

        return data?.ConvertToDto();
    }

    public async Task<ListingDetailsDto?> GetListingDetailsAsync(int id)
    {
        var data = await _dbContext.Listings
            .Include(x => x.CreatedBy)
            .FirstOrDefaultAsync(x => x.Id == id);
        return data == null
            ? null
            : new ListingDetailsDto
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

    public async Task<List<ListingDetailsDto>> GetAllListingDetailsAsync()
    {
        var data = await _dbContext.Listings
            .Include(x => x.CreatedBy)
            .Select(x => new ListingDetailsDto
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

    public async Task<List<ListingDto>> GetAllListingsAsync()
    {
        var data = await _dbContext.Listings.ToListAsync();
        
        return data.Select(x => x.ConvertToDto()).ToList();
    }
}