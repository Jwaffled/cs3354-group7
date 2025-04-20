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


    public async Task<int> CreateForumReplyAsync(int forumPostId, CreateForumReplyDto dto)
    {
        var reply = new Models.ForumReply
        {
            Message = dto.Message,
            ForumPostId = forumPostId,
        };
        
        _dbContext.ForumReplies.Add(reply); 
        
        await _dbContext.SaveChangesAsync();

        return reply.Id;
    }

    public async Task<ForumReplyListItemDto?> GetForumReplyByIdAsync(int replyId)
    {
        var data = await _dbContext.ForumReplies
            .AsNoTracking()
            .Include(x => x.CreatedBy)
            .FirstOrDefaultAsync(x => x.Id == replyId);

        return data == null ? null : new ForumReplyListItemDto
        {
            Id = data.Id,
            AuthorId = data.CreatedById,
            AuthorName = data.CreatedBy.FirstName + " " + data.CreatedBy.LastName,
            CreatedAt = data.CreatedAt,
            Message = data.Message,
        };
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