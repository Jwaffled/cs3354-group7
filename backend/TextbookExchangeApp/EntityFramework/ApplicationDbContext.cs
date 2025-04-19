using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TextbookExchangeApp.Models;
using TextbookExchangeApp.Models.Contracts;

namespace TextbookExchangeApp.EntityFramework;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Listing> Listings { get; set; }
    public DbSet<Reply> Replies { get; set; }
    public DbSet<ForumPost> ForumPosts { get; set; }
    public DbSet<ForumReply> ForumReplies { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Add other model configurations here
        builder.Entity<Reply>()
            .HasOne(x => x.Recipient)
            .WithMany(x => x.RepliesReceived)
            .HasForeignKey(x => x.RecipientId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Reply>()
            .HasOne(x => x.CreatedBy)
            .WithMany(x => x.RepliesCreated)
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Listing>()
            .HasOne(x => x.CreatedBy)
            .WithMany(x => x.Listings)
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<ForumPost>()
            .HasOne(x => x.CreatedBy)
            .WithMany(x => x.ForumPosts)
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ForumReply>()
            .HasOne(x => x.CreatedBy)
            .WithMany(x => x.ForumReplies)
            .HasForeignKey(x => x.CreatedById);

        builder.Entity<ForumReply>()
            .HasOne(x => x.ForumPost)
            .WithMany(x => x.ForumReplies)
            .HasForeignKey(x => x.ForumPostId);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Added && e.Entity is IAuditableEntity);
        
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        foreach (var entry in entries)
        {
            var entity = (IAuditableEntity)entry.Entity;
            
            entity.CreatedById = userId;
            entity.CreatedAt = DateTime.UtcNow;
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}