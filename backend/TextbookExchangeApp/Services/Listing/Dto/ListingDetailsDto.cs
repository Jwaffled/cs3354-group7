namespace TextbookExchangeApp.Services.Listing.Dto;

public class ListingDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Condition { get; set; }
    public double Price { get; set; }
    public string CreatedById { get; set; }
    public string AuthorName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ImageUrl { get; set; }
}