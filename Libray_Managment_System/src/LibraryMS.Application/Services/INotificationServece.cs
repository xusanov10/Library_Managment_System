using LibraryMS.Application.Models.Notification;

namespace LibraryMS.Application.Services
{
    public interface INotificationService
    {
        Task<string> CreateNotificationAsync(NotificationDTO dto);
        Task<IEnumerable<NotificationDTO>> GetUserNotificationsAsync(int userId);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<string> DeleteNotificationAsync(int id);
    }
}