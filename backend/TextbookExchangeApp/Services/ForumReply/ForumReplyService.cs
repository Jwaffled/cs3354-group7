using TextbookExchangeApp.EntityFramework;
using TextbookExchangeApp.Services.ForumReply.Dto;

namespace TextbookExchangeApp.Services.ForumReply;

public class ForumReplyService : IForumReplyService
{
    private readonly ApplicationDbContext _dbContext;

    public ForumReplyService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task CreateForumReplyAsync(int forumPostId, CreateForumReplyDto dto)
    {
        _dbContext.ForumReplies.Add(new Models.ForumReply
        {
            Message = dto.Message,
            ForumPostId = forumPostId,
        }); 
        
        await _dbContext.SaveChangesAsync();
    }
}