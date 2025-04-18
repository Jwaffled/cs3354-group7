using Microsoft.AspNetCore.Identity;
using TextbookExchangeApp.Services.Login.Dto;

namespace TextbookExchangeApp.Services.Login;

public interface ILoginService
{
    Task<IdentityResult> CreateAccountAsync(CreateAccountDto dto);
    Task<bool> LoginUserAsync(AccountLoginDto dto);
    Task<ProfileDataDto?> GetProfileDataAsync(string profileId);
}