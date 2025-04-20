using TextbookExchangeApp.Services.Reply.Dto;

namespace TextbookExchangeApp.Services.Reply;

public interface IReplyService
{
    Task<int> CreateReplyAsync(string profileId, CreateReplyDto dto);
    Task<ReplyListItemDto?> GetReplyByIdAsync(int id);
    Task<List<ReplyListItemDto>> GetAllRepliesAsync(string profileId);
}