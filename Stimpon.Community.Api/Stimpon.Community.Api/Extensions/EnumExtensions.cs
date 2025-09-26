// File namespace
namespace Stimpon.Community.Api;

/// <summary>
/// Contains extension methods for enums
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Return the role as a string
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public static string Text(this Roles role) => role.ToString();
}

