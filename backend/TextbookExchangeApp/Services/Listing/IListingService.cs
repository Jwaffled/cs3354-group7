using TextbookExchangeApp.Services.Listing.Dto;

namespace TextbookExchangeApp.Services.Listing;

public interface IListingService
{
    Task CreateListingAsync(ListingDto dto);
    Task<ListingDto?> GetListingByIdAsync(int id);
    Task<List<ListingDto>> GetAllListingsAsync();
    Task<ListingDetailsDto?> GetListingDetailsAsync(int id);
    Task<List<ListingDetailsDto>> GetAllListingDetailsAsync();
}