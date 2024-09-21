namespace ChatAppClone.Core
{
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using ChatAppClone.Data.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class UserFollowsService : IUserFollowsService
    {
        private readonly IRepository repository;

        public UserFollowsService(IRepository _repository)
        {
            this.repository = _repository;
        }

        public async Task<bool> FollowUserAsync(string userIdToFollow, string currentUserId)
        {
            UserFollows? existingFollow = await this.repository.AllReadonly<UserFollows>()
                                               .FirstOrDefaultAsync(uf => uf.UserId == userIdToFollow && uf.FollowerId == currentUserId);

            if (existingFollow != null)
            {
                return false;
            }

            UserFollows follow = new UserFollows
            {
                UserId = userIdToFollow,
                FollowerId = currentUserId,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            };

            await this.repository.AddAsync(follow);
            await this.repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnfollowUserAsync(string userIdToUnfollow, string currentUserId)
        {
            UserFollows? follow = await this.repository.AllReadonly<UserFollows>()
                                       .FirstOrDefaultAsync(uf => uf.UserId == userIdToUnfollow && uf.FollowerId == currentUserId);

            if (follow == null)
            {
                return false;
            }

            await this.repository.DeleteAsync<UserFollows>(follow.Id);
            await this.repository.SaveChangesAsync();

            return true;
        }
    }
}
