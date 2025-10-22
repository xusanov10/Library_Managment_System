using LibraryMS.Application.Models;
using LibraryMS.Application.Models.Notification;
using Libray_Managment_System.Services;

namespace LibraryMS.Application.Services
{
    public interface INotificationService
    {
        Task<Result<string>> CreateNotificationAsync(CreateNotificationDTO createNotificationDTO);
        Task<Result<IEnumerable<NotificationListResponseDTO>>> GetUserNotificationsAsync();
        Task<Result<UserResponseDTO>> GetUserById(int id);
        Task<Result<bool>> MarkAsReadAsync(int notificationId);
        Task<Result<string>> DeleteNotificationAsync(int id);
    }
}