// File namespace 
namespace Stimpon.Community.Api.V1.Contracts;

// Required namespaces
using System.ComponentModel.DataAnnotations;

/// <summary>
/// A community thread
/// </summary>
public class ThreadDto
{
    /// <summary>
    /// Thread id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Thread header
    /// </summary>
    public string? Header { get; set; }

    /// <summary>
    /// Thread text
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// The thread category
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Is this thread resolved
    /// </summary>
    public bool Resolved { get; set; }

    /// <summary>
    /// When was the thread created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The owner for this thread
    /// </summary>
    public string? Owner { get; set; }

    /// <summary>
    /// The owner's id for this thread
    /// </summary>
    public int OwnerId { get; set; }
}
