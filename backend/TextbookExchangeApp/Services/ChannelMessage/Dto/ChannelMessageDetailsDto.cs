namespace TextbookExchangeApp.Services.ChannelMessage.Dto;

public class ChannelMessageDetailsDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string AuthorName { get; set; }
    public string AuthorId { get; set; }
}