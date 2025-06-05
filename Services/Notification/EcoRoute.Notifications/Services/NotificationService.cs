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
        private readonly ILogger<NotificationService> _logger;
        public NotificationService(
                  NotificationsContext context,
                  IMapper mapper,
                  IHubContext<NotificationHub> hubContext,
                  IHttpContextAccessor httpContextAccessor,
                  ILogger<NotificationService> logger) 
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<ResultNotificationDto> CreateAsync(CreateNotificationDto dto)
        {
            var notificationEntity = _mapper.Map<Notification>(dto);
            _context.Notifications.Add(notificationEntity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ResultNotificationDto>(notificationEntity);

            _logger.LogInformation(
                "Publishing notification. UserId={UserId}, UserRole={UserRole}",
                dto.UserId, dto.UserRole);

            if (!string.IsNullOrWhiteSpace(dto.UserId))
            {
                await _hubContext.Clients
                                .Group(dto.UserId)
                                .SendAsync("ReceiveNotification", result);
            }

            if (!string.IsNullOrWhiteSpace(dto.UserRole))
            {
                var roles = dto.UserRole
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => r.Trim())
                    .Distinct(); 

                foreach (var role in roles)
                {
                    await _hubContext.Clients
                        .Group("Role_" + role)
                        .SendAsync("ReceiveNotification", result);
                }
            }



            if (string.IsNullOrWhiteSpace(dto.UserId)
             && string.IsNullOrWhiteSpace(dto.UserRole))
            {
                await _hubContext.Clients
                                .Group("AllUsers")
                                .SendAsync("ReceiveNotification", result);
            }

            return result;
        }


        public async Task<List<ResultNotificationDto>> GetAllForUserAsync(string userId)
        {
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
                    n.UserId == userId ||

                    (!string.IsNullOrEmpty(n.UserRole) &&
                    userRoles.Any(role => n.UserRole.Contains(role))))
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();

            return _mapper.Map<List<ResultNotificationDto>>(notifications);
        }

        public async Task<List<ResultNotificationDto>> GetUnreadForUserAsync(string userId)
        {
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
                    (n.UserId == userId && !n.IsRead) ||
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
                    (n.UserId == userId && !n.IsRead) ||
                    (!string.IsNullOrEmpty(n.UserRole) &&
                     userRoles.Any(role => n.UserRole.Contains(role)) &&
                     !n.IsRead))
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
                    (n.UserId == userId && !n.IsRead) ||
                    (!string.IsNullOrEmpty(n.UserRole) &&
                     userRoles.Any(role => n.UserRole.Contains(role)) &&
                     !n.IsRead));
        }
    }
}