// File namespace
namespace Stimpon.Community.Api.V1.Contracts;

/// <summary>
/// Model for updating a comment answer
/// </summary>
public class UpdateCommentAnswerDto
{
    /// <summary>
    /// Is this comment an answer
    /// </summary>
    public bool IsAnswer { get; set; }
}
