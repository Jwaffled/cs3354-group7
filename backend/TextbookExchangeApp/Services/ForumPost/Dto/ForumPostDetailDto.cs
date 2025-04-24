namespace TextbookExchangeApp.Services.ForumPost.Dto;

public class ForumPostDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string AuthorName { get; set; }
    public string AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
}