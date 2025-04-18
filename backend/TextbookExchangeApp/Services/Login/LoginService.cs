using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TextbookExchangeApp.EntityFramework;
using TextbookExchangeApp.Models;
using TextbookExchangeApp.Services.Login.Dto;

namespace TextbookExchangeApp.Services.Login;

public class LoginService : ILoginService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _dbContext;

    public LoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
    }

    public async Task<IdentityResult> CreateAccountAsync(CreateAccountDto dto)
    {
        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email, FirstName = dto.FirstName, LastName = dto.LastName };
        return await _userManager.CreateAsync(user, dto.Password);
    }

    public async Task<bool> LoginUserAsync(AccountLoginDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, false);
        return result.Succeeded;
    }

    public async Task<ProfileDataDto?> GetProfileDataAsync(string profileId)
    {
        var user = await _dbContext.Users
            .Include(x => x.RepliesReceived)
            .FirstOrDefaultAsync(u => u.Id == profileId);

        return user != null
            ? new ProfileDataDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AverageRating = user.RepliesReceived.Average(x => x.Rating)
            }
            : null;
    }
}