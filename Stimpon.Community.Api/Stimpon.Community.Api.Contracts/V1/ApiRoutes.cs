// File namespace
namespace Stimpon.Community.Api.V1.Contracts;

/// <summary>
/// Contains api routes
/// </summary>
public static class ApiRoutes
{
    /// <summary>
    /// The base API route
    /// </summary>
    public const string BaseRoute = "api/v1";

    /// <summary>
    /// Contains routes for the auth controller
    /// </summary>
    public static class AuthControllerRoutes
    {
        /// <summary>
        /// The base controller route
        /// </summary>
        public const string ControllerRoute = $"{BaseRoute}/auth";

        /// <summary>
        /// Route for the register endpoint
        /// </summary>
        public const string RegisterRoute = $"{ControllerRoute}/register";

        /// <summary>
        /// Route for the sign in endpoint
        /// </summary>
        public const string SignInRoute = $"{ControllerRoute}/sign-in";

        /// <summary>
        /// Route for the sign out endpoint
        /// </summary>
        public const string SignOutRoute = $"{ControllerRoute}/sign-out";
    }

    /// <summary>
    /// Contains routes for the users controller
    /// </summary>
    public static class UsersControllerRoutes
    {
        /// <summary>
        /// The base controller route
        /// </summary>
        public const string ControllerRoute = $"{BaseRoute}/users";

        /// <summary>
        /// Route for the me andpoint
        /// </summary>
        public const string MeRoute = $"{ControllerRoute}/me";
    }

    public static class ThreadsControllerRoutes
    {
        /// <summary>
        /// The base controller route
        /// </summary>
        public const string ControllerRoute = $"{BaseRoute}/threads";

        /// <summary>
        /// Get thread by id
        /// </summary>
        public const string GetThreadByIdRoute = ControllerRoute + "/{id}";

        /// <summary>
        /// Get all threads
        /// </summary>
        public const string GetAllThreads = ControllerRoute;

        /// <summary>
        /// Delete a thread by id
        /// </summary>
        public const string DeleteThreadByIdRoute = ControllerRoute + "/{id}";

        /// <summary>
        /// The create thread route
        /// </summary>
        public const string CreateThreadRoute = ControllerRoute;

        /// <summary>
        /// Resolve a thread by id
        /// </summary>
        public const string ResolveThreadByIdRoute = ControllerRoute + "/{id}/resolve";
    }

    public static class CommentsControllerRoutes
    {
        /// <summary>
        /// Route for creating a comment under a thread
        /// </summary>
        public const string CreateCommentOnAThreadRoute = BaseRoute + "/threads/{threadId}/comments";

        /// <summary>
        /// Route for creating a comment under a thread
        /// </summary>
        public const string CreateSubComment = BaseRoute + "/comments/{commentId}/comments";

        /// <summary>
        /// Route for fetching all comments under a thread
        /// </summary>
        public const string GetAllThreadComments = BaseRoute + "/threads/{threadId}/comments";

        /// <summary>
        /// Route for fetching all sub-comments under a comment
        /// </summary>
        public const string GetSubComments = BaseRoute + "/Comments/{commentId}/comments";

        /// <summary>
        /// Route for the change asnwer state of a comment
        /// </summary>
        public const string UpdateCommentAnswerState = BaseRoute + "/comments/{commentId}/answer";
    }
}
