using System.ComponentModel.DataAnnotations;
using TextbookExchangeApp.Models;

namespace TextbookExchangeApp.Services.Listing.Dto;

public class ListingDto
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public Enums.TextbookCondition Condition { get; set; }

    public string AuthorId { get; set; }

    public Models.Listing ConvertToModel()
    {
        return new Models.Listing
        {
            Id = Id,
            Title = Title,
            Description = Description,
            Price = Price,
            Condition = Condition,
            AuthorId = AuthorId
        };
    }
}