namespace TextbookExchangeApp.Services.Reply.Dto;

public class ReplyDto
{
    public int Id {  get; set; }
    public string Message { get; set; }
    public int Rating { get; set; }
    public string? RecipientId { get; set; }
    public string? CreatedById { get; set; }
    public DateTime? CreatedAt { get; set; }
}