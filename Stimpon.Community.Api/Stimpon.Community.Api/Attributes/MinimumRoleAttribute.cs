// File namespace
namespace Stimpon.Community.Api;

// Required namespaces
using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Allows a user with a minimum role to access an endpoint
/// </summary>
public class MinimumRoleAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="role"></param>
    public MinimumRoleAttribute(Roles role)
    {
        // Set the minimum role policy
        Policy = $"MINIMUM_ROLE_{role.Text()}";
    }
}
