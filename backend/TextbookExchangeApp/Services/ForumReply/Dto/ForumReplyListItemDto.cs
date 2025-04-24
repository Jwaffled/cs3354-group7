namespace TextbookExchangeApp.Services.ForumReply.Dto;

public class ForumReplyListItemDto
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string AuthorName { get; set; }
    public string AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
}