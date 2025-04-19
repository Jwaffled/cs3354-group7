using TextbookExchangeApp.Services.ForumReply.Dto;

namespace TextbookExchangeApp.Services.ForumReply;

public interface IForumReplyService
{
    Task CreateForumReplyAsync(int forumPostId, CreateForumReplyDto dto);
    Task<List<ForumReplyListItemDto>> GetForumPostRepliesAsync(int forumPostId);
}