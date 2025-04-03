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
        var data = dto.ConvertToModel();
        _dbContext.Replies.Add(data);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ReplyDto?> GetReplyByIdAsync(int id)
    {
        var data = await _dbContext.Replies.FirstOrDefaultAsync(x => x.Id == id);

        return data?.ConvertToDto();
    }

    public async Task<List<ReplyDto>> GetAllRepliesAsync(int listingId)
    {
        var data = await _dbContext.Replies.Where(x => x.ListingId == listingId).ToListAsync();
        return data.Select(x => x.ConvertToDto()).ToList();
    }

    public async Task<List<ReplyDto>> GetAllRepliesAsync()
    {
        var data = await _dbContext.Replies.ToListAsync();
        return data.Select(x => x.ConvertToDto()).ToList();
    }
}