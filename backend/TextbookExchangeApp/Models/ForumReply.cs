using TextbookExchangeApp.Models.Contracts;

namespace TextbookExchangeApp.Models;

public class ForumReply : IAuditableEntity
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Foreign Keys
    public string CreatedById { get; set; }

    public int ForumPostId { get; set; }
    // Navigation Properties
    
    public ApplicationUser CreatedBy { get; set; }
    public ForumPost ForumPost { get; set; }
}