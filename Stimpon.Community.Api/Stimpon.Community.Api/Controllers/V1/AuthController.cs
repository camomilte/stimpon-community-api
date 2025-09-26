// File namespace
namespace Stimpon.Community.Api.V1;

// Required namespaces
using Microsoft.AspNetCore.Mvc;
using Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

/// <summary>
/// Authentication controller
/// </summary>
public class AuthController(CommunityDbContext context, IConfiguration configuration) : BaseController
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <returns></returns>
    [HttpPost(ApiRoutes.AuthControllerRoutes.RegisterRoute)]
    public async Task<ActionResult<UserDto>> Register([FromBody]RegisterUserDto request)
    {
        #region User already exists?

        // Check if there already is a user with this email
        if (await context.Users.AnyAsync(u => u.Email == request.Email))
        {
            // Return bad request
            return BadRequest(new
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request",
                Title = "Email is already in use",
                Status = StatusCodes.Status400BadRequest,
                Detail = "An account with this user already exists",
                Instance = ApiRoutes.AuthControllerRoutes.RegisterRoute
            });
        }

        #endregion

        // Create a hasher
        var hasher = new PasswordHasher<User>();

        // Create the user data object
        var user = new User 
        { 
            Email = request.Email,        // Save email from the request
            Username = request.Username,  // Save username for the request
            Role = Roles.User,            // User will have the most basic authority by default
        };

        // Generate password hash
        user.Password = hasher.HashPassword(user, request.Password!);

        // Add the user to the database
        var createdUser = context.Users.Add(user);
        // Save database changes
        await context.SaveChangesAsync();

        // Return the created user
        return Ok(new UserDto()
        {
            Id  = createdUser.Entity.Id,
            Email = createdUser.Entity.Email,
            Username = createdUser.Entity.Username,
            Role = createdUser.Entity.Role.Text()
        });
    }

    /// <summary>
    /// Sign in a user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost(ApiRoutes.AuthControllerRoutes.SignInRoute)]
    public async Task<ActionResult> Login([FromBody]LoginUserDto request)
    {
        // Find the user with this email
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        // If the user was found
        if (user is not null)
        {
            // Create a hasher
            var hasher = new PasswordHasher<User>();

            // Check if password is correct
            var res = hasher.VerifyHashedPassword(user, user.Password!, request.Password!);

            // If password is correct
            if(res == PasswordVerificationResult.Success)
            {
                // Create the token response
                var response = new LoginResponseDto()
                {
                    AccessToken = GenerateAuthToken(user),
                };

                // Return the token
                return Ok(response);
            }
        }

        // Return bad request
        return Unauthorized(new
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.2",
            Title = "Unauthorized",
            Status = StatusCodes.Status401Unauthorized,
            Detail = "Wrong email or password",
            Instance = ApiRoutes.AuthControllerRoutes.SignInRoute
        });
    }

    /// <summary>
    /// Generate an auth token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private string GenerateAuthToken(User user)
    {
        // Create the claims list
        var claims = new List<Claim>
        {
            // Create claim with the user id
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            // Create claim with the role
            new Claim(ClaimTypes.Role, user.Role.Text())
        };

        // Get the secret key from appsettings
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AuthTokenData:Secret")!));

        // Create the signature signer
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("AuthTokenData:Issuer")!,
            audience: configuration.GetValue<string>("AuthTokenData:Audience")!,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(int.Parse(configuration["AuthTokenData:TokenValidityTimeInSeconds"]!)),
            signingCredentials: creds
        );

        // Create the token and return it
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
