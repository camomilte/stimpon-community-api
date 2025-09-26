// File namespace
namespace Stimpon.Community.Api;

// Required namespaces
using System.ComponentModel.DataAnnotations;

/// <summary>
/// A community thread
/// </summary>
public class Thread
{
    /// <summary>
    /// Thread id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Thread header
    /// </summary>
    [MaxLength(128)]
    [Required]
    public string? Header { get; set; }

    /// <summary>
    /// Thread text
    /// </summary>
    [MaxLength(4096)]
    [Required]
    public string? Text { get; set; }

    /// <summary>
    /// The thread category
    /// </summary>
    [MaxLength(64)]
    [Required]
    public string? Category { get; set; }

    /// <summary>
    /// Is this thread resolved
    /// </summary>
    [Required]
    public bool Resolved { get; set; }

    /// <summary>
    /// When was the thread created
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The user who created this thread
    /// </summary>
    [Required]
    public int OwnerId { get; set; }
    /// <summary>
    /// The user who created this thread
    /// </summary>
    [Required]
    public User? Owner { get; set; }

    /// <summary>
    /// All comments under this thread
    /// </summary>
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
