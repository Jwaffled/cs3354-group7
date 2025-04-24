using Microsoft.EntityFrameworkCore;
using TextbookExchangeApp.EntityFramework;
using TextbookExchangeApp.Services.Reply.Dto;

namespace TextbookExchangeApp.Services.Reply;

public class ReplyService : IReplyService
{
    private readonly ApplicationDbContext _dbContext;

    public ReplyService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<int> CreateReplyAsync(string profileId, CreateReplyDto dto)
    {
        if (dto.Rating <= 0 || dto.Rating > 5)
        {
            throw new ArgumentException("Rating must be between 0 and 5.");
        }

        var reply = new Models.Reply
        {
            RecipientId = profileId,
            Message = dto.Message,
            Rating = dto.Rating,
        };

        _dbContext.Replies.Add(reply);
        
        await _dbContext.SaveChangesAsync();

        return reply.Id;
    }

    public async Task<ReplyListItemDto?> GetReplyByIdAsync(int id)
    {
        var data = await _dbContext.Replies
            .AsNoTracking()
            .Include(x => x.CreatedBy)
            .FirstOrDefaultAsync(x => x.Id == id);

        return data == null ? null : new ReplyListItemDto
        {
            Id = data.Id,
            CreatedAt = data.CreatedAt,
            Message = data.Message,
            Rating = data.Rating,
            AuthorName = data.CreatedBy.FirstName + " " + data.CreatedBy.LastName,
        };
    }

    public async Task<List<ReplyListItemDto>> GetAllRepliesAsync(string profileId)
    {
        var data = await _dbContext.Replies
            .AsNoTracking()
            .Where(x => x.RecipientId == profileId)
            .Select(x => new ReplyListItemDto
            {
                Id = x.Id,
                Message = x.Message,
                Rating = x.Rating,
                AuthorName = x.CreatedBy.FirstName + " " + x.CreatedBy.LastName,
                CreatedAt = x.CreatedAt,
            }).ToListAsync();

        return data;
    }
}