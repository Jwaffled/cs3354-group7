using TextbookExchangeApp.Services.Channel.Dto;

namespace TextbookExchangeApp.Services.Channel;

public interface IChannelService
{
    Task<ChannelDetailsDto> GetOrCreateChannelAsync(List<string> userIds);
    Task<List<ChannelDetailsDto>> GetChannelsForUserAsync(string userId);
    Task<ChannelDetailsDto?> GetChannelByIdAsync(int id);
}