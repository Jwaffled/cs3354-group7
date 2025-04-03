using Microsoft.AspNetCore.Identity;
using TextbookExchangeApp.Models;
using TextbookExchangeApp.Services.Login.Dto;

namespace TextbookExchangeApp.Services.Login;

public class LoginService : ILoginService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IdentityResult> CreateAccountAsync(CreateAccountDto dto)
    {
        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
        return await _userManager.CreateAsync(user, dto.Password);
    }

    public async Task<bool> LoginUserAsync(AccountLoginDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, false);
        return result.Succeeded;
    }
}