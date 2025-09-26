// File namespace
namespace Stimpon.Community.Api.V1.Contracts;

/// <summary>
/// User data object
/// </summary>
public class UserDto
{
    /// <summary>
    /// The user id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The username
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// The user email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The user role
    /// </summary>
    public string? Role { get; set; }
}