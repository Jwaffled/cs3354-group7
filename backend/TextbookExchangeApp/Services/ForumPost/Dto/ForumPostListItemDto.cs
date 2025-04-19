namespace TextbookExchangeApp.Services.ForumPost.Dto;

public class ForumPostListItemDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string AuthorName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ReplyCount { get; set; }
    public string Preview { get; set; }
}