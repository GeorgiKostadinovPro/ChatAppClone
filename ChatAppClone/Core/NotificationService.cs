namespace ChatAppClone.Core
{
    using ChatAppClone.Core.Contracts;
    using ChatAppClone.Data.Models;
    using ChatAppClone.Data.Repositories;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class NotificationService : INotificationService
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;

        public NotificationService(IRepository _repository, UserManager<ApplicationUser> _userManager)
        {
            this.repository = _repository;
            this.userManager = _userManager;
        }

        public async Task CreateNotificationAsync(string content, string url, string userId)
        {
            var notification = new Notification
            {
                UserId = userId,
                Content = content,
                Url = url,
                IsRead = false,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            };

            await this.repository.AddAsync(notification);
            await this.repository.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(string notificationId)
        {
            Notification? notification = await this.repository
                .AllReadonly<Notification>()
                .FirstOrDefaultAsync(n => n.Id.ToString() == notificationId);

            if (notification != null)
            {
                await this.repository.DeleteAsync<Notification>(notification.Id);
                await this.repository.SaveChangesAsync();
            }
        }

        public async Task<int> GetNotificationsCountByUserId(string userId)
        {
            return await this.repository.AllReadonly<Notification>()
                .CountAsync(n => n.UserId == userId);
        }
    }
}
