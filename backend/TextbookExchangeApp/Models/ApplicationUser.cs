using Microsoft.AspNetCore.Identity;

namespace TextbookExchangeApp.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Navigation Properties
    public List<Listing> Listings { get; set; }
    public List<Reply> RepliesCreated { get; set; }
    public List<Reply> RepliesReceived { get; set; }
}