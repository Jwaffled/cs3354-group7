using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TextbookExchangeApp.Services.Reply.Dto;

namespace TextbookExchangeApp.Models;

public class Reply
{
    public int Id { get; set; }
    public string Message { get; set; }
    // Foreign Keys
    public int ListingId { get; set; }
    public string AuthorId { get; set; }

    // Navigation Properties
    public ApplicationUser Author { get; set; }
    public Listing Listing { get; set; }

    public ReplyDto ConvertToDto()
    {
        return new ReplyDto
        {
            Id = Id,
            Message = Message,
            ListingId = ListingId,
            AuthorId = AuthorId,
        };
    }
}

