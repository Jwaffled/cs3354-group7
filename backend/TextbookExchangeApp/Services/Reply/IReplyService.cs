using TextbookExchangeApp.Services.Reply.Dto;

namespace TextbookExchangeApp.Services.Reply;

public interface IReplyService
{
    Task CreateReplyAsync(ReplyDto dto);
    Task<ReplyDto?> GetReplyByIdAsync(int id);
    Task<List<ReplyDetailsDto>> GetAllRepliesAsync(int listingId);
    Task<List<ReplyDto>> GetAllRepliesAsync();
}