// File namespace
namespace Stimpon.Community.Api;

// Required namespaces
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a user in the database
/// </summary>
public class User
{
    /// <summary>
    /// User primary key
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The username
    /// </summary>
    [Required]
    public string? Username { get; set; }

    /// <summary>
    /// The user email
    /// </summary>
    [Required]
    public string? Email { get; set; }

    /// <summary>
    /// The user password
    /// </summary>
    [Required]
    public string? Password { get; set; }

    /// <summary>
    /// The user role
    /// </summary>
    [Required]
    public Roles Role { get; set; }

    /// <summary>
    /// Threads for this user
    /// </summary>
    public ICollection<Thread> Threads { get; set; } = new List<Thread>();

    /// <summary>
    /// All the users comments
    /// </summary>
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
