// File namespace
namespace Stimpon.Community.Api.V1.Contracts;

// Required namespaces
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Login user Request
/// </summary>
public class LoginUserDto
{
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