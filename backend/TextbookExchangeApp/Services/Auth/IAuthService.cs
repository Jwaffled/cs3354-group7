using Microsoft.AspNetCore.Identity;
using TextbookExchangeApp.Services.Auth.Dto;
using TextbookExchangeApp.Services.Profile.Dto;

namespace TextbookExchangeApp.Services.Auth;

public interface IAuthService
{
    Task<IdentityResult> CreateAccountAsync(CreateAccountDto dto);
    Task<bool> LoginUserAsync(AccountLoginDto dto);
}