namespace ChatAppClone.Core.Contracts
{
    public interface IUserFollowsService
    {
        Task<bool> FollowUserAsync(string userIdToFollow, string currentUserId);

        Task<bool> UnfollowUserAsync(string userIdToUnfollow, string currentUserId);
    }
}
