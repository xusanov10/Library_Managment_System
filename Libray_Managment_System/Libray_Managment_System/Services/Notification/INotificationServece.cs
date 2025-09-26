using Libray_Managment_System.DtoModels;

using Libray_Managment_System.DtoModels;

namespace Library_Management_System.Services
{
    public interface INotificationService
    {
        Task<string> CreateNotificationAsync(NotificationDTO dto);
        Task<IEnumerable<NotificationDTO>> GetUserNotificationsAsync(int userId);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<string> DeleteNotificationAsync(int id);
    }
}