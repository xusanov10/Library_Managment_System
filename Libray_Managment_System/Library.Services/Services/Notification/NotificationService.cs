using Library_Managment_System;
using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;
using Library_Managment_System1;
namespace Library_Management_System.Services
{
    public class NotificationService : INotificationService
    {
        private readonly LibraryManagmentSystemContext _context;

        public NotificationService(LibraryManagmentSystemContext context)
        {
            _context = context;
        }

        public async Task<string> CreateNotificationAsync(NotificationDTO dto)
        {
            var notification = new Notification
            {
                Userid = dto.UserId,
                Message = dto.Message,
                Isread = false,
                Createdat = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            return "Notification created successfully!";
        }

        public async Task<IEnumerable<NotificationDTO>> GetUserNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.Userid == userId)
                .Select(n => new NotificationDTO
                {
                    Id = n.Id,
                    UserId = n.Userid,
                    Message = n.Message,
                    IsRead = n.Isread,
                    CreatedAt = n.Createdat ?? DateTime.MinValue
                })
                .ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
                return false;

            notification.Isread = true;
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<string> DeleteNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
                return "Notification not found!";

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return "Notification deleted successfully!";
        }
    }
}
