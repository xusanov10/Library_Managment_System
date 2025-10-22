using Library_Managment_System;
using Library_Managment_System1;
using LibraryMS.Application.Models.Notification;
using LibraryMS.Application.Services;
using Libray_Managment_System.Models;
using Libray_Managment_System.Services;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services
{
    public class NotificationService : INotificationService
    {
        private readonly LibraryManagmentSystemContext _context;

        public NotificationService(LibraryManagmentSystemContext context)
        {
            _context = context;
        }

        public async Task<Result<string>> CreateNotificationAsync(CreateNotificationDTO dto)
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
            return new Result<string>
            {
                Message = "Notification created successfully!",
                StatusCode = 201,
            };
        }

        public async Task<Result<IEnumerable<NotificationListResponseDTO>>> GetUserNotificationsAsync()
        {
            var notifications = await _context.Notifications
                .Select(n => new NotificationListResponseDTO
                {
                    Id = n.Id,
                    UserId = n.Userid,
                    Message = n.Message,
                    IsRead = n.Isread,
                    CreatedAt = n.Createdat.Value
                })
                .ToListAsync();
            return new Result<IEnumerable<NotificationListResponseDTO>>
                {
                Data = notifications,
                Message = "Notifications retrieved successfully!",
                StatusCode = 200,
            };
        }

        public async Task<Result<bool>> MarkAsReadAsync(int notificationId)
        {
          var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return new Result<bool>
                {
                    Data = false,
                    Message = "Notification not found!",
                    StatusCode = 404,
                };
            }
            notification.Isread = true;
            await _context.SaveChangesAsync();
            return new Result<bool>
            {
                Data = true,
                Message = "Notification marked as read!",
                StatusCode = 200,
            };
        }

        public async Task<Result<string>> DeleteNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return new Result<string>
                {
                    Message = "Notification not found!",
                    StatusCode = 404,
                };
            }
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return new Result<string>
            {
                Message = "Notification deleted successfully!",
                StatusCode = 200,
            };
        }
        public async Task<Result<UserResponseDTO>> GetUserById(int id)
        {
            var user = _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserResponseDTO
                {
                    Id = u.Id,
                    Fullname = u.Fullname,
                    Email = u.Email,
                    Status = u.Status
                })
                .FirstOrDefaultAsync();

            return new Result<UserResponseDTO>
            {
                Data = user,
                Message = "User retrieved successfully!",
                StatusCode = 200,
            };
        }
    }
}
