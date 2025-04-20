using TextbookExchangeApp.Services.ForumPost.Dto;

namespace TextbookExchangeApp.Services.ForumPost;

public interface IForumPostService
{
    Task<int> CreateForumPostAsync(CreateForumPostDto dto);
    Task<List<ForumPostListItemDto>> GetAllForumPostsAsync();
    Task<ForumPostDetailDto?> GetForumPostByIdAsync(int id);
}