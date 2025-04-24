using System.ComponentModel.DataAnnotations;

namespace TextbookExchangeApp.Services.Listing.Dto;

public class CreateListingDto
{
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative.")]
    public double? Price { get; set; }
    [Required]
    public Enums.TextbookCondition? Condition { get; set; }
    public string ImageUrl { get; set; }
}