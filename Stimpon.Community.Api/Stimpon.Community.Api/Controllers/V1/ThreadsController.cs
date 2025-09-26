// File namespace
namespace Stimpon.Community.Api.V1;

// Required namespaces
using Microsoft.AspNetCore.Mvc;
using Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

/// <summary>
/// Threads controller
/// </summary>
public class ThreadsController(CommunityDbContext context) : BaseController
{
    #region Endpoints

    /// <summary>
    /// Get a single thread by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.ThreadsControllerRoutes.GetThreadByIdRoute, Name = nameof(ApiRoutes.ThreadsControllerRoutes.GetThreadByIdRoute))]
    public async Task<ActionResult<ThreadDto>> GetThreadById(int id)
    {
        // Get the thread from the database
        var thread = await context.Threads.FindAsync(id);

        // If the thread is found, return it
        if (thread is not null) 
            return new ThreadDto 
            { 
                Id = thread.Id,
                Header = thread.Header, 
                Text = thread.Text, 
                Resolved = thread.Resolved,
                CreatedAt = thread.CreatedAt,
                Category = thread.Category,
                Owner = (await context.Users.FirstOrDefaultAsync(u => u.Id == thread.OwnerId))?.Username ?? "DELETED_USER",
                OwnerId = thread.OwnerId
             };
        // Otherwise return not found
        else return new NotFoundResult();
    }

    /// <summary>
    /// Get all threads
    /// </summary>
    /// <returns></returns>
    [HttpGet(ApiRoutes.ThreadsControllerRoutes.GetAllThreads)]
    public async Task<ActionResult<List<ThreadDto>>> GetAllThreads()
    {
        // Return all threads
        return (await context.Threads.ToListAsync()).Select(thread => new ThreadDto
        {
            Id = thread.Id,
            Header = thread.Header,
            Text = thread.Text,
            Category = thread.Category,
            Resolved = thread.Resolved,
            CreatedAt = thread.CreatedAt,
            Owner = context.Users.FirstOrDefault(user => user.Id == thread.OwnerId)?.Username ?? "DELETED_USER",
            OwnerId = thread.OwnerId
        }).ToList();
    }

    /// <summary>
    /// Create a new thread
    /// </summary>
    /// <returns></returns>
    [MinimumRole(Roles.User)]
    [HttpPost(ApiRoutes.ThreadsControllerRoutes.CreateThreadRoute)]
    public async Task<ActionResult<ThreadDto>> CreateThread([FromBody]CreateThreadDto thread)
    {
        // Get the user id from the token
        var userId = int.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        // Find the user who are creating the thread
        var user = await context.Users.FindAsync(userId);

        // If the user was not found, should never happen
        if(user is null) return new StatusCodeResult(StatusCodes.Status500InternalServerError);

        // Create the thread
        var createdThread = context.Threads.Add(new Thread
        {
            // We know these are not null because of model validation
            Header = thread.Header,
            Text = thread.Text,
            Category = thread.Category,
            CreatedAt = DateTime.UtcNow,
            Resolved = false,
            OwnerId = user.Id,
        });
        // Save database changes
        await context.SaveChangesAsync();
        
        // Return the created thread
        return new CreatedAtRouteResult(
            routeName: nameof(ApiRoutes.ThreadsControllerRoutes.GetThreadByIdRoute),
            routeValues: new { id = createdThread.Entity.Id },
            value: new ThreadDto
            {
                Id = createdThread.Entity.Id,
                Header = createdThread.Entity.Header,
                Text = createdThread.Entity.Text,
                Category = createdThread.Entity.Category,
                CreatedAt = createdThread.Entity.CreatedAt,
                Resolved = createdThread.Entity.Resolved,
                Owner = user.Username,
                OwnerId = user.Id
            });
    }

    /// <summary>
    /// Delete a thread
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [MinimumRole(Roles.User)]
    [HttpDelete(ApiRoutes.ThreadsControllerRoutes.DeleteThreadByIdRoute)]
    public async Task<ActionResult> DeleteThread(int id)
    {
        // Find the thread to delete
        var thread = context.Threads.Find(id);

        // Return 404 if the thread is not found
        if(thread is null) return new NotFoundResult();

        // If this is a regular user, then he should only be able to delete his own threads
        if (!IsUserAllowed(thread.OwnerId)) return Forbid();

        // Remove the thread
        context.Threads.Remove(thread);
        // Save database changes
        await context.SaveChangesAsync();

        // Return 202 no content
        return new NoContentResult();
    }

    /// <summary>
    /// Resolve a thread
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [MinimumRole(Roles.User)]
    [HttpPost(ApiRoutes.ThreadsControllerRoutes.ResolveThreadByIdRoute)]
    public async Task<ActionResult> ResolveThread(int id)
    {
        // Find the thread to resolve
        var thread = await context.Threads.FindAsync(id);
        // Return 404 if the thread is not found
        if (thread is null) return new NotFoundResult();

        // If this is a regular user, then he should only be able to resolve his own threads
        if (!IsUserAllowed(thread.OwnerId)) return Forbid();

        // Mark the thread as resolved
        thread.Resolved = true;
        // Save database changes
        await context.SaveChangesAsync();

        // Return 202 no content
        return new NoContentResult();
    }

    /// <summary>
    /// Check if a user is allowed a specific action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected bool IsUserAllowed(int id)
    {
        // If this is a regular user, then he should only be able to delete his own threads
        if (Enum.Parse<Roles>(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Role).Value) < Roles.Moderator)
        {
            // This is a regular user

            // Check if this thread is owned by the requested user
            if (id != int.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value))
            {
                // This is not a thread that this user created

                // Return forbidden result
                return false;
            }
        }

        // User is allowed
        return true;
    }

    #endregion
}

