// File namespace
namespace Stimpon.Community.Api;

// Required namespaces
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Thread database context
/// </summary>
public class CommunityDbContext(DbContextOptions<CommunityDbContext> options) : DbContext(options)
{
    /// <summary>
    /// The thread set
    /// </summary>
    public DbSet<Thread> Threads => Set<Thread>();

    /// <summary>
    /// The users set
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// All the comments
    /// </summary>
    public DbSet<Comment> Comments { get; set; }

    /// <summary>
    /// On model creating
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1 to many relationship (Multiple threads per user)
        modelBuilder.Entity<Thread>()
            .HasOne(t => t.Owner)
            .WithMany(u => u.Threads)
            .HasForeignKey(t => t.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Comment>()
            .HasOne(t => t.Owner)
            .WithMany(u => u.Comments)
            .HasForeignKey(t => t.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Comment>()
            .HasOne(t => t.ParentThread)
            .WithMany(u => u.Comments)          
            .HasForeignKey(t => t.ParentThreadId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(u => u.Comments)
            .HasForeignKey(t => t.ParentCommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

