using TextbookExchangeApp.Services.Listing.Dto;

namespace TextbookExchangeApp.Models;

public class Listing
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public Enums.TextbookCondition Condition { get; set; }
    
    // Foreign Keys
    public string AuthorId { get; set; }

    // Navigation Properties
    public ApplicationUser Author { get; set; }
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
            AuthorId = AuthorId,
        };
    }
}