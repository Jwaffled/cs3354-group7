namespace TextbookExchangeApp.Models.Contracts;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    string? CreatedById { get; set; }
}