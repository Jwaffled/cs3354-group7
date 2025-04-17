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
    
    public async Task CreateReplyAsync(ReplyDto dto)
    {
        if (dto.Rating <= 0 || dto.Rating > 5)
        {
            throw new ArgumentException("Rating must be between 0 and 5.");
        }
        var data = dto.ConvertToModel();
        _dbContext.Replies.Add(data);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ReplyDto?> GetReplyByIdAsync(int id)
    {
        var data = await _dbContext.Replies.FirstOrDefaultAsync(x => x.Id == id);

        return data?.ConvertToDto();
    }

    public async Task<List<ReplyDetailsDto>> GetAllRepliesAsync(int listingId)
    {
        var data = await _dbContext.Replies
            .Include(x => x.CreatedBy)
            .Where(x => x.ListingId == listingId).ToListAsync();
        
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
        return data.Select(x => x.ConvertToDto()).ToList();
    }
}