// File namespace
namespace Stimpon.Community.Api.V1;

using Contracts;
// Required namespaces
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

/// <summary>
/// Users controller
/// </summary>
public class UsersController(CommunityDbContext context) : BaseController
{
    /// <summary>
    /// Returns the signed on user
    /// </summary>
    /// <returns></returns>
    [HttpGet(ApiRoutes.UsersControllerRoutes.MeRoute)]
    [MinimumRole(Roles.User)]
    public async Task<ActionResult<UserDto>> Me()
    {
        // Find the signed on user
        var user = await context.FindAsync<User>(
            int.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value));

        // If the user is not signed on, return unauthorized
        if (user is null) return Unauthorized();

        // Return the user dto
        return Ok(new UserDto()
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            Role = user.Role.Text()
        });
    }
}
