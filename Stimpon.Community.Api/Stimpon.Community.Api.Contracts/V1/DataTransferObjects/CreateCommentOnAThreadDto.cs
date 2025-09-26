// File namespace
namespace Stimpon.Community.Api.V1;

// Required namespaces
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Create comment on a thread
/// </summary>
public class CreateCommentOnAThreadDto
{
    /// <summary>
    /// comment text
    /// </summary>
    [MaxLength(4096)]
    [Required]
    public string? Text { get; set; }
}
