namespace ChatAppClone.Common.Constants
{
    public static class MiddlewareConstants
    {
        public const string ErrorDuringExecutingPath = "Error during executing {Path}";

        public const string UserApiPath = "/api/UserApi";

        public const string ApplicationJson = "application/json";

        public static readonly Dictionary<int, string> RedirectURLs = new() 
        {
            { 400, "/Home/Error?statusCode=400" },
            { 401, "/Home/Error?statusCode=401" },
            { 403, "/Home/Error?statusCode=403" },
            { 404, "/Home/Error?statusCode=404" },
            { 500, "/Home/Error?statusCode=500" }
        };
    }
}
