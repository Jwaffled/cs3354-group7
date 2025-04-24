using Microsoft.EntityFrameworkCore;
using TextbookExchangeApp.EntityFramework;
using TextbookExchangeApp.Services.Profile.Dto;

namespace TextbookExchangeApp.Services.Profile;

public class ProfileService : IProfileService
{
    private readonly ApplicationDbContext _dbContext;

    public ProfileService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ProfileDataDto?> GetProfileDataAsync(string userId)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Include(x => x.RepliesReceived)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user != null
            ? new ProfileDataDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AverageRating = user.RepliesReceived.Count != 0 ? user.RepliesReceived.Average(x => x.Rating) : 0
            }
            : null;
    }

    public async Task<List<ProfileDataDto>> GetAllProfileDataAsync()
    {
        var users = await _dbContext.Users
            .AsNoTracking()
            .Select(x => new ProfileDataDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                AverageRating = x.RepliesReceived.Count != 0 ? x.RepliesReceived.Average(y => y.Rating) : 0
            })
            .ToListAsync();

        return users;
    }
}