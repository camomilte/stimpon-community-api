// File namespace
using System.ComponentModel.DataAnnotations;

namespace Stimpon.Community.Api.V1.Contracts;

/// <summary>
/// A comment
/// </summary>
public class CommentDto
{
    /// <summary>
    /// Comment id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Thread text
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// When was the thread created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Is this comment marked as an answer
    /// </summary>
    public bool Answer { get; set; }

    /// <summary>
    /// The parent thread id
    /// </summary>
    public int ParentThreadId { get; set; }

    /// <summary>
    /// The parent comment id if not thread scoped
    /// </summary>
    public int? ParentCommentId { get; set; }

    /// <summary>
    /// The user who created this thread
    /// </summary>
    public int OwnerId { get; set; }
    /// <summary>
    /// The user who created this thread
    /// </summary>
    public string? Owner { get; set; }
}
