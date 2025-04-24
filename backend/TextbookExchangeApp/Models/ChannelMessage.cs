using TextbookExchangeApp.Models.Contracts;

namespace TextbookExchangeApp.Models;

public class ChannelMessage : IAuditableEntity
{
    public int Id { get; set; }
    public int ChannelId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedById { get; set; }
    
    public ApplicationUser CreatedBy { get; set; }
    public Channel Channel { get; set; }
}