using Microsoft.EntityFrameworkCore;
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

    public async Task<List<ForumReplyListItemDto>> GetForumPostRepliesAsync(int forumPostId)
    {
        var data = await _dbContext.ForumReplies
            .AsNoTracking()
            .Where(x => x.ForumPostId == forumPostId)
            .Select(x => new ForumReplyListItemDto
            {
                Id = x.Id,
                AuthorId = x.CreatedById,
                AuthorName = x.CreatedBy.FirstName + " " + x.CreatedBy.LastName,
                CreatedAt = x.CreatedAt,
                Message = x.Message,
            }).ToListAsync();

        return data;
    }
}