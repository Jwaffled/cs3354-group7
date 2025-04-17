using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TextbookExchangeApp.Models.Contracts;
using TextbookExchangeApp.Services.Reply.Dto;

namespace TextbookExchangeApp.Models;

public class Reply : IAuditableEntity
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Message { get; set; }
    // Foreign Keys
    public int ListingId { get; set; }
    public string CreatedById { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation Properties
    public ApplicationUser CreatedBy { get; set; }
    public Listing Listing { get; set; }

    public ReplyDto ConvertToDto()
    {
        return new ReplyDto
        {
            Id = Id,
            Message = Message,
            Rating = Rating,
            ListingId = ListingId,
            CreatedById = CreatedById,
            CreatedAt = CreatedAt,
        };
    }
}

