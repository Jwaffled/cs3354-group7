using TextbookExchangeApp.Models.Contracts;

namespace TextbookExchangeApp.Models;

public class ForumReply : IAuditableEntity
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string CreatedById { get; set; }
    public DateTime CreatedAt { get; set; }
}