using TextbookExchangeApp.Services.ForumReply.Dto;

namespace TextbookExchangeApp.Services.ForumReply;

public interface IForumReplyService
{
    Task<int> CreateForumReplyAsync(int forumPostId, CreateForumReplyDto dto);
    Task<ForumReplyListItemDto?> GetForumReplyByIdAsync(int replyId);
    Task<List<ForumReplyListItemDto>> GetForumPostRepliesAsync(int forumPostId);
}