namespace TextbookExchangeApp.Services.Channel.Dto;

public class ChannelDetailsDto
{
    public int Id { get; set; }
    public List<string> ChannelMemberNames { get; set; }
    public string? LastMessage { get; set; }
    public DateTime? LastMessageDate { get; set; }
}