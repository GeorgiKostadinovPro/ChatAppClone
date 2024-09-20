namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Data.Models;
    using ChatAppClone.Models.ViewModels.Notifications;

    public interface INotificationService
    {
        Task CreateNotificationAsync(string content, string url, string userId);

        Task DeleteNotificationAsync(string notificationId);

        Task<int> GetNotificationsCountByUserId(string userId);

        Task<IEnumerable<NotificationViewModel>> GetNotificationsAsync(string userId, int currPage);
    }
}
