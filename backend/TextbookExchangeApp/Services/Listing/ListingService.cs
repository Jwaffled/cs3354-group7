using Microsoft.EntityFrameworkCore;
using TextbookExchangeApp.EntityFramework;
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

    public async Task<List<ListingDto>> GetAllListingsAsync()
    {
        var data = await _dbContext.Listings.ToListAsync();
        
        return data.Select(x => x.ConvertToDto()).ToList();
    }
}