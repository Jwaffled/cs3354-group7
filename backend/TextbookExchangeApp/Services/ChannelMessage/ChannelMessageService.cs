using Microsoft.EntityFrameworkCore;
using TextbookExchangeApp.EntityFramework;
using TextbookExchangeApp.Services.ChannelMessage.Dto;

namespace TextbookExchangeApp.Services.ChannelMessage;

public class ChannelMessageService : IChannelMessageService
{
    private readonly ApplicationDbContext _dbContext;

    public ChannelMessageService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ChannelMessageDetailsDto> SendMessageAsync(int channelId, string senderId,
        SendChannelMessageDto dto)
    {
        var message = new Models.ChannelMessage
        {
            ChannelId = channelId,
            Content = dto.Message,
        };

        _dbContext.ChannelMessages.Add(message);
        
        await _dbContext.SaveChangesAsync();
        
        var messageData = await _dbContext.ChannelMessages
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == message.Id);

        return new ChannelMessageDetailsDto
        {
            Id = messageData.Id
        };
    }

    public async Task<List<ChannelMessageDetailsDto>> GetMessagesForChannelAsync(int channelId)
    {
        var messages = await _dbContext.ChannelMessages
            .AsNoTracking()
            .Where(x => x.ChannelId == channelId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();

        return messages.Select(x => new ChannelMessageDetailsDto
        {
            Id = x.Id,
        }).ToList();
    }
}