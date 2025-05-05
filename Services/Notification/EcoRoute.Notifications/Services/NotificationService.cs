using AutoMapper;
using EcoRoute.Notifications.Context;
using EcoRoute.Notifications.Dtos;
using EcoRoute.Notifications.Entities;
using EcoRoute.Notifications.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcoRoute.Notifications.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationsContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationService(
                  NotificationsContext context,
                  IMapper mapper,
                  IHubContext<NotificationHub> hubContext,
                  IHttpContextAccessor httpContextAccessor) // Eklenmesi gereken parametre
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResultNotificationDto> CreateAsync(CreateNotificationDto dto)
        {
            var notification = _mapper.Map<Notification>(dto);
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ResultNotificationDto>(notification);

            // Send notification through SignalR
            if (!string.IsNullOrEmpty(dto.UserId))
            {
                // Send to specific user
                await _hubContext.Clients.Group(dto.UserId).SendAsync("ReceiveNotification", result);
            }
            else
            {
                // Send to all users
                await _hubContext.Clients.Group("AllUsers").SendAsync("ReceiveNotification", result);
            }

            return result;
        }

        public async Task<List<ResultNotificationDto>> GetAllForUserAsync(string userId)
        {
            // Kullanıcı rollerini al
            var userRoles = new List<string>();

            if (_httpContextAccessor.HttpContext != null)
            {
                var user = _httpContextAccessor.HttpContext.User;
                userRoles = user.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();
            }

            // Filtreleme mantığı
            var notifications = await _context.Notifications
                .Where(n =>
                    // UserId ile gönderilen bildirimler (sadece belirli kullanıcı için)
                    n.UserId == userId ||

                    // Rolüne göre gönderilen bildirimler
                    (!string.IsNullOrEmpty(n.UserRole) &&
                    userRoles.Any(role => n.UserRole.Contains(role))))
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();

            return _mapper.Map<List<ResultNotificationDto>>(notifications);
        }

        public async Task<List<ResultNotificationDto>> GetUnreadForUserAsync(string userId)
        {
            // Kullanıcı rollerini al
            var userRoles = new List<string>();

            if (_httpContextAccessor.HttpContext != null)
            {
                var user = _httpContextAccessor.HttpContext.User;
                userRoles = user.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();
            }

            var notifications = await _context.Notifications
                .Where(n =>
                    // UserId ile gönderilen bildirimleri göster
                    (n.UserId == userId && !n.IsRead) ||
                    // Kullanıcının rollerine göre gönderilen bildirimleri göster
                    (!string.IsNullOrEmpty(n.UserRole) &&
                     userRoles.Any(role => n.UserRole.Contains(role)) &&
                     !n.IsRead))
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();

            return _mapper.Map<List<ResultNotificationDto>>(notifications);
        }

        public async Task<bool> MarkAsReadAsync(Guid id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
                return false;

            notification.IsRead = true;
            notification.ReadDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(string userId)
        {
            var notifications = await _context.Notifications
                .Where(n => (n.UserId == userId || n.UserId == null) && !n.IsRead)
                .ToListAsync();

            if (!notifications.Any())
                return false;

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            // Kullanıcı rollerini al
            var userRoles = new List<string>();

            if (_httpContextAccessor.HttpContext != null)
            {
                var user = _httpContextAccessor.HttpContext.User;
                userRoles = user.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();
            }

            return await _context.Notifications
                .CountAsync(n =>
                    // UserId ile gönderilen bildirimler
                    (n.UserId == userId && !n.IsRead) ||
                    // Kullanıcının rollerine göre gönderilen bildirimler
                    (!string.IsNullOrEmpty(n.UserRole) &&
                     userRoles.Any(role => n.UserRole.Contains(role)) &&
                     !n.IsRead));
        }
    }
}