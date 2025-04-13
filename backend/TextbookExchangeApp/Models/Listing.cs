using TextbookExchangeApp.Models.Contracts;
using TextbookExchangeApp.Services.Listing.Dto;

namespace TextbookExchangeApp.Models;

public class Listing : IAuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public Enums.TextbookCondition Condition { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedById { get; set; }
    public string ImageUrl { get; set; }

    // Navigation Properties
    public ApplicationUser CreatedBy { get; set; }
    public List<Reply> Replies { get; set; }

    public ListingDto ConvertToDto()
    {
        return new ListingDto
        {
            Id = Id,
            Title = Title,
            Description = Description,
            Price = Price,
            Condition = Condition,
            ImageUrl = ImageUrl,
        };
    }
}