using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TextbookExchangeApp.Models;

namespace TextbookExchangeApp.EntityFramework;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<TextbookListing> listings { get; set; }
    public DbSet<reply> allReplies { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Add other model configurations here
        builder.Entity<reply>()
            .HasOne(postReply => postReply.listing)
            .WithMany(bkListing => bkListing.replyList)
            .HasForeignKey(postReply => postReply.listingID)
            .OnDelete(DeleteBehavior.SetNull);
    }
}