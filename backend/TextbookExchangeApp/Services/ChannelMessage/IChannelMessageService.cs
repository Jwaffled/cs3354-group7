using TextbookExchangeApp.Services.ChannelMessage.Dto;

namespace TextbookExchangeApp.Services.ChannelMessage;

public interface IChannelMessageService
{
    Task<ChannelMessageDetailsDto> SendMessageAsync(int channelId, string senderId, SendChannelMessageDto dto);
    Task<List<ChannelMessageDetailsDto>> GetMessagesForChannelAsync(int channelId);
}