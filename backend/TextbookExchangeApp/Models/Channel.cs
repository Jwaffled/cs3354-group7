namespace TextbookExchangeApp.Models;

public class Channel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<ChannelUser> ChannelUsers { get; set; }
    public List<ChannelMessage> ChannelMessages { get; set; }
}