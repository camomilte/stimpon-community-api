// File namespace
namespace Stimpon.Community.Api;

// Required namespaces
using System.ComponentModel.DataAnnotations;

/// <summary>
/// A comment on a thread or another comment
/// </summary>
public class Comment
{
    /// <summary>
    /// Comment id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Thread text
    /// </summary>
    [MaxLength(4096)]
    [Required]
    public string? Text { get; set; }

    /// <summary>
    /// When was the thread created
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Is this comment marked as an answer or not
    /// </summary>
    public bool Answer { get; set; }

    /// <summary>
    /// The parent thread id
    /// </summary>
    public int ParentThreadId { get; set; }
    /// <summary>
    /// The parent thread
    /// </summary>
    public Thread? ParentThread { get; set; }

    /// <summary>
    /// The parent comment id if not thread scoped
    /// </summary>
    public int? ParentCommentId { get; set; }

    /// <summary>
    /// The parent commeent if not thread scoped
    /// </summary>
    public Comment? ParentComment { get; set; }

    /// <summary>
    /// All comments under this commecnt
    /// </summary>
    public ICollection<Comment>? Comments { get; set; } = new List<Comment>();

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
}
