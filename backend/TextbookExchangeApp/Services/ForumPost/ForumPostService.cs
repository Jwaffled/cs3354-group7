using Microsoft.EntityFrameworkCore;
using TextbookExchangeApp.EntityFramework;
using TextbookExchangeApp.Services.ForumPost.Dto;

namespace TextbookExchangeApp.Services.ForumPost;

public class ForumPostService : IForumPostService
{
    private readonly ApplicationDbContext _dbContext;

    public ForumPostService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateForumPostAsync(CreateForumPostDto dto)
    {
        _dbContext.ForumPosts.Add(new Models.ForumPost
        {
            Title = dto.Title,
            Description = dto.Description,
        });
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<ForumPostListItemDto>> GetAllForumPostsAsync()
    {
        var data = await _dbContext.ForumPosts
            .AsNoTracking()
            .Select(x => new ForumPostListItemDto
            {
                Id = x.Id,
                AuthorName = x.CreatedBy.FirstName + " " + x.CreatedBy.LastName,
                CreatedAt = x.CreatedAt,
                Preview = x.Description.Length > 100 ? x.Description.Substring(0, 100) : x.Description,
                ReplyCount = x.ForumReplies.Count,
                Title = x.Title
            })
            .ToListAsync();

        return data;
    }

    public async Task<ForumPostDetailDto?> GetForumPostByIdAsync(int id)
    {
        var data = await _dbContext.ForumPosts
            .AsNoTracking()
            .Include(x => x.CreatedBy)
            .FirstOrDefaultAsync(x => x.Id == id);

        return data == null
            ? null
            : new ForumPostDetailDto
            {
                Id = data.Id,
                AuthorId = data.CreatedBy.Id,
                AuthorName = data.CreatedBy.FirstName + " " + data.CreatedBy.LastName,
                CreatedAt = data.CreatedAt,
                Description = data.Description,
                Title = data.Title,
            };
    }
}