namespace TextbookExchangeApp.Services.Reply.Dto;

public class ReplyDetailsDto
{
    public int Id { get; set; }
    public string Message { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public string AuthorName { get; set; }
}