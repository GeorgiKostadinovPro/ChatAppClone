namespace ChatAppClone.Core.Contracts
{
    using ChatAppClone.Models.ViewModels.Notifications;

    public interface INotificationService
    {
        Task CreateAsync(string content, string url, string userId);

        Task DeleteAsync(Guid notificationId);

        Task<int> GetCountByUserId(string userId);

        Task<IEnumerable<NotificationViewModel>> GetAsync(string userId, int currPage);
    }
}
