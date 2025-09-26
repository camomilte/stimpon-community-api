// File namespace
namespace Stimpon.Community.Api.V1;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stimpon.Community.Api.V1.Contracts;
using System.ComponentModel.Design;
// Required namespaces
using System.Security.Claims;

/// <summary>
/// Comments controller
/// </summary>
/// <param name="context"></param>
public class CommentsController(CommunityDbContext context) : BaseController
{
    /// <summary>
    /// Create a comment on a thread
    /// </summary>
    /// <returns></returns>
    [MinimumRole(Roles.User)]
    [HttpPost(ApiRoutes.CommentsControllerRoutes.CreateCommentOnAThreadRoute)]
    public async Task<ActionResult<CommentDto>> CreateComment(int threadId, [FromBody]CreateCommentOnAThreadDto request)
    {
        // Find the thread to create the comment under
        var thread = await context.Threads.FindAsync(threadId);
        // Return 404 if the thread is not found
        if (thread is null) return new NotFoundResult();
        // Check if thread is resolved
        if (thread.Resolved) return new ForbidResult();

        // Get the user id from the token
        var userId = int.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        // Create the thread
        var createdComment = await context.Comments.AddAsync(new Comment()
        {
            Text = request.Text,
            CreatedAt = DateTime.UtcNow,
            Answer = false,
            OwnerId = userId,
            ParentThreadId = threadId
        });
        // Save database changes
        await context.SaveChangesAsync();

        // Return the created comment
        return new CommentDto()
        {
            Id  = createdComment.Entity.Id,
            Text = createdComment.Entity.Text,
            CreatedAt = createdComment.Entity.CreatedAt,
            Answer = createdComment.Entity.Answer,
            ParentThreadId = createdComment.Entity.ParentThreadId,
            OwnerId = createdComment.Entity.OwnerId,
            Owner = (await context.Users.FindAsync(userId))?.Username ?? "DELETED_USER"
        };
    }

    /// <summary>
    /// Create a comment on a thread
    /// </summary>
    /// <returns></returns>
    [MinimumRole(Roles.User)]
    [HttpPost(ApiRoutes.CommentsControllerRoutes.CreateSubComment)]
    public async Task<ActionResult<CommentDto>> CreateSubComment(int commentId, [FromBody] CreateCommentOnAThreadDto request)
    {
        // Find the comment to delete
        var comment = await context.Comments.FindAsync(commentId);
        // Return 404 if the comment is not found
        if (comment is null) return new NotFoundResult();

        // Get the user id from the token
        var userId = int.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        // Create the thread
        var createdComment = await context.Comments.AddAsync(new Comment()
        {
            Text = request.Text,
            CreatedAt = DateTime.UtcNow,
            Answer = false,
            OwnerId = userId,
            ParentThreadId = comment.ParentThreadId,
            ParentCommentId = commentId
        });
        // Save database changes
        await context.SaveChangesAsync();

        // Return the created comment
        return new CommentDto()
        {
            Id = createdComment.Entity.Id,
            Text = createdComment.Entity.Text,
            CreatedAt = createdComment.Entity.CreatedAt,
            Answer = createdComment.Entity.Answer,
            ParentThreadId = createdComment.Entity.ParentThreadId,
            ParentCommentId = createdComment.Entity.ParentCommentId,
            OwnerId = createdComment.Entity.OwnerId,
            Owner = (await context.Users.FindAsync(userId))?.Username ?? "DELETED_USER"
        };
    }

    /// <summary>
    /// Fetch all comments under a thread
    /// </summary>
    /// <param name="threadId"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.CommentsControllerRoutes.GetAllThreadComments)]
    public async Task<ActionResult<List<CommentDto>>> GetThreadComments(int threadId)
    {
        // Find the thread to delete
        var thread = await context.Threads.FindAsync(threadId);
        // Return 404 if the thread is not found
        if (thread is null) return new NotFoundResult();

        // Return all comments under this thread
        var comments = await context.Comments
            .Where(c => c.ParentThreadId == threadId && c.ParentCommentId == null) // Only top-level comments
            .Include(c => c.Owner)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Text = c.Text,
                CreatedAt = c.CreatedAt,
                Answer = c.Answer,
                ParentThreadId = c.ParentThreadId,
                ParentCommentId = c.ParentCommentId,
                OwnerId = c.OwnerId,
                Owner = c.Owner != null ? c.Owner.Username : "DELETED_USER"
            })
            .ToListAsync();

        // Return all comments
        return comments;
    }

    /// <summary>
    /// Get comments under a comment
    /// </summary>
    /// <param name="commentId"></param>
    /// <returns></returns>
    [HttpGet(ApiRoutes.CommentsControllerRoutes.GetSubComments)]
    public async Task<ActionResult<List<CommentDto>>> GetSubComments(int commentId)
    {
        // Check if the requested comment exists
        if (await context.Comments.FindAsync(commentId) is null) return new NotFoundResult();

        // Fetch all sub comments under the requested comment
        var subComments = await context.Comments
            .Where(c => c.ParentCommentId == commentId)
            .Include(c => c.Owner)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Text = c.Text,
                CreatedAt = c.CreatedAt,
                Answer = c.Answer,
                ParentThreadId = c.ParentThreadId,
                ParentCommentId = c.ParentCommentId,
                OwnerId = c.OwnerId,
                Owner = c.Owner != null ? c.Owner.Username : "DELETED_USER"
            })
            .ToListAsync();

        // Return the comments
        return subComments;
    }

    /// <summary>
    /// Set the answer state of a comment
    /// </summary>
    /// <returns></returns>
    [MinimumRole(Roles.User)]
    [HttpPatch(ApiRoutes.CommentsControllerRoutes.UpdateCommentAnswerState)]
    public async Task<IActionResult> MarkCommentAsAnswerRoute(int commentId, [FromBody]UpdateCommentAnswerDto request)
    {
        // Find the comment to mark as answer
        var comment = await context.Comments.FindAsync(commentId);

        // Return 404 if the comment is not found
        if (comment is null) return new NotFoundResult();

        // Get the user id from the token
        var userId = int.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        // Find the parent thread
        var parentThread = await context.Threads.FindAsync(comment?.ParentThreadId);


        // Return forbidden result if the requesting user is not the owner of the parent thread
        if (parentThread?.OwnerId != userId) return new ForbidResult();

        // Mark the comment as answer
        comment!.Answer = request.IsAnswer;
        // Update the comment
        await context.SaveChangesAsync();

        // Return 202 no content
        return new NoContentResult();
    }
}

