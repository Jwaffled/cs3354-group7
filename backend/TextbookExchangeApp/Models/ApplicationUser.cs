using Microsoft.AspNetCore.Identity;

namespace TextbookExchangeApp.Models;

public class ApplicationUser : IdentityUser
{
    // Navigation Properties
    public List<Listing> Listings { get; set; }
    public List<Reply> Replies { get; set; }
}