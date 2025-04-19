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
    
    public async Task CreateReplyAsync(string profileId, CreateReplyDto dto)
    {
        if (dto.Rating <= 0 || dto.Rating > 5)
        {
            throw new ArgumentException("Rating must be between 0 and 5.");
        }
        
        _dbContext.Replies.Add(new Models.Reply
        {
            RecipientId = profileId,
            Message = dto.Message,
            Rating = dto.Rating,
        });
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ReplyDto?> GetReplyByIdAsync(int id)
    {
        var data = await _dbContext.Replies.FirstOrDefaultAsync(x => x.Id == id);

        return data == null ? null : new ReplyDto
        {
            Id = data.Id,
            CreatedAt = data.CreatedAt,
            CreatedById = data.CreatedById,
            Message = data.Message,
            Rating = data.Rating,
            RecipientId = data.RecipientId,
        };
    }

    public async Task<List<ReplyDetailsDto>> GetAllRepliesAsync(string profileId)
    {
        var data = await _dbContext.Replies
            .Include(x => x.CreatedBy)
            .Where(x => x.RecipientId == profileId).ToListAsync();
        
        return data.Select(x => new ReplyDetailsDto
            {
                Id = x.Id,
                Message = x.Message,
                Rating = x.Rating,
                AuthorName = x.CreatedBy.FirstName + " " + x.CreatedBy.LastName,
                CreatedAt = x.CreatedAt,
            })
            .ToList();
    }

    public async Task<List<ReplyDto>> GetAllRepliesAsync()
    {
        var data = await _dbContext.Replies.ToListAsync();
        return data.Select(x => new ReplyDto
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            CreatedById = x.CreatedById,
            Message = x.Message,
            Rating = x.Rating,
            RecipientId = x.RecipientId,
        }).ToList();
    }
}