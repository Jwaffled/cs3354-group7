using TextbookExchangeApp.Models.Contracts;

namespace TextbookExchangeApp.Models;

public class ForumPost : IAuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    // Foreign Keys
    public string CreatedById { get; set; }
    
    // Navigation properties
    public ApplicationUser CreatedBy { get; set; }
    public List<ForumReply> ForumReplies { get; set; }
}