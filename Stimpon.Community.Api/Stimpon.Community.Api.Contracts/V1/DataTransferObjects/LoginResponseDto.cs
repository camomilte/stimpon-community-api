// File namespace 
namespace Stimpon.Community.Api.V1.Contracts;

/// <summary>
/// The login request data object
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// The access token
    /// </summary>
    public string? AccessToken { get; set; }
}
