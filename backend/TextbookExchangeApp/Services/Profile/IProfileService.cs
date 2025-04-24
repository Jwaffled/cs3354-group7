using TextbookExchangeApp.Services.Profile.Dto;

namespace TextbookExchangeApp.Services.Profile;

public interface IProfileService
{
    Task<ProfileDataDto?> GetProfileDataAsync(string userId);
    Task<List<ProfileDataDto>> GetAllProfileDataAsync();
}