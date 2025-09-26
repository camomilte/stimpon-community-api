// File namespace
namespace Stimpon.Community.Api.V1.Contracts;

// Required namespaces
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Register user Request
/// </summary>
public class RegisterUserDto
{
    /// <summary>
    /// The username
    /// </summary>
    [Required]
    public string? Username { get; set; }

    /// <summary>
    /// The user email
    /// </summary>
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// The user password
    /// </summary>
    [Required]
    public string? Password { get; set; }
}

