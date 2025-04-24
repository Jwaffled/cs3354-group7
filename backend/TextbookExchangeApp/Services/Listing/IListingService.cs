using TextbookExchangeApp.Services.Listing.Dto;

namespace TextbookExchangeApp.Services.Listing;

public interface IListingService
{
    Task<int> CreateListingAsync(CreateListingDto dto);
    Task<ListingListItemDto?> GetListingByIdAsync(int id);
    Task<ListingListItemDto?> GetListingDetailsAsync(int id);
    Task<List<ListingListItemDto>> GetAllListingsAsync();
}