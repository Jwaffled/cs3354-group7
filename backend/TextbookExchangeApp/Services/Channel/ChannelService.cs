using Microsoft.EntityFrameworkCore;
using TextbookExchangeApp.EntityFramework;
using TextbookExchangeApp.Services.Channel.Dto;

namespace TextbookExchangeApp.Services.Channel;

public class ChannelService : IChannelService
{
    private readonly ApplicationDbContext _dbContext;

    public ChannelService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ChannelDetailsDto> GetOrCreateChannelAsync(List<string> userIds)
    {
        var channels = await _dbContext.Channels
            .AsNoTracking()
            .Include(x => x.ChannelUsers).ThenInclude(x => x.User)
            .Where(x => x.ChannelUsers.Count == userIds.Count &&
                        x.ChannelUsers.All(y => userIds.Contains(y.UserId)))
            .ToListAsync();

        var existing = channels
            .FirstOrDefault(x => userIds.All(y => x.ChannelUsers.Any(z => z.UserId == y)));
        
        

        if (existing != null)
        {
            var lastMessage = await _dbContext.ChannelMessages
                .Where(m => m.ChannelId == existing.Id)
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefaultAsync();
            
            return new ChannelDetailsDto
            {
                Id = existing.Id,
                ChannelMemberNames = existing.ChannelUsers
                    .Select(cu => cu.User.FirstName + " " + cu.User.LastName)
                    .ToList(),
                LastMessageDate = lastMessage?.CreatedAt,
                LastMessage = lastMessage?.Content
            };
        }

        var channel = new Models.Channel
        {
            ChannelUsers = userIds.Select(x => new Models.ChannelUser
            {
                UserId = x
            }).ToList(),
            CreatedAt = DateTime.UtcNow,
        };
        
        _dbContext.Channels.Add(channel);
        await _dbContext.SaveChangesAsync();

        var channelData = await _dbContext.Channels
            .AsNoTracking()
            .Include(x => x.ChannelUsers).ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == channel.Id);
        
        return new ChannelDetailsDto
        {
            Id = channelData.Id,
            ChannelMemberNames = channelData.ChannelUsers
                .Select(cu => cu.User.FirstName + " " + cu.User.LastName)
                .ToList(),
        };
    }

    public async Task<List<ChannelDetailsDto>> GetChannelsForUserAsync(string userId)
    {
        var channels = await _dbContext.Channels
            .AsNoTracking()
            .Where(c => c.ChannelUsers.Any(cu => cu.UserId == userId))
            .Select(c => new
            {
                c.Id,
                MemberNames = c.ChannelUsers
                    .Select(cu => cu.User.FirstName + " " + cu.User.LastName)
                    .ToList(),

                LastMessage = _dbContext.ChannelMessages
                    .Where(m => m.ChannelId == c.Id)
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => new
                    {
                        m.CreatedAt,
                        m.Content
                    })
                    .FirstOrDefault()
            })
            .ToListAsync();

        return channels.Select(c => new ChannelDetailsDto
        {
            Id = c.Id,
            ChannelMemberNames = c.MemberNames,
            LastMessageDate = c.LastMessage?.CreatedAt,
            LastMessage = c.LastMessage?.Content,
        }).ToList();
    }

    public async Task<ChannelDetailsDto?> GetChannelByIdAsync(int id)
    {
        var channel = await _dbContext.Channels
            .AsNoTracking()
            .Include(x => x.ChannelUsers).ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        var lastMessage = await _dbContext.ChannelMessages
            .Where(m => m.ChannelId == id)
            .OrderByDescending(m => m.CreatedAt)
            .FirstOrDefaultAsync();
        
        

        return channel == null ? null : new ChannelDetailsDto
        {
            Id = channel.Id,
            ChannelMemberNames = channel.ChannelUsers
                .Select(cu => cu.User.FirstName + " " + cu.User.LastName)
                .ToList(),
            LastMessageDate = lastMessage?.CreatedAt,
            LastMessage = lastMessage?.Content,
        };
    }
}