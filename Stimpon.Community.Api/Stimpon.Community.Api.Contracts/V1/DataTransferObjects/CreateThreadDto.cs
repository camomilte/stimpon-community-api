// File namespace 
namespace Stimpon.Community.Api.V1.Contracts;

// Required namespaces
using System.ComponentModel.DataAnnotations;

/// <summary>
/// A community thread
/// </summary>
public class CreateThreadDto
{
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
}
