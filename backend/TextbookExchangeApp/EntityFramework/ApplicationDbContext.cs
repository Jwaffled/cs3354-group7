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

    public DbSet<Listing> Listings { get; set; }
    public DbSet<Reply> Replies { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Add other model configurations here
        builder.Entity<Reply>()
            .HasOne(x => x.Listing)
            .WithMany(x => x.Replies)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Reply>()
            .HasOne(x => x.Author)
            .WithMany(x => x.Replies)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Listing>()
            .HasOne(x => x.Author)
            .WithMany(x => x.Listings)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}