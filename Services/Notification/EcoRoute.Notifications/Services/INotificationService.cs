using EcoRoute.Notifications.Dtos;

namespace EcoRoute.Notifications.Services
{
    public interface INotificationService
    {
        Task<ResultNotificationDto> CreateAsync(CreateNotificationDto dto);
        Task<List<ResultNotificationDto>> GetAllForUserAsync(string userId);
        Task<List<ResultNotificationDto>> GetUnreadForUserAsync(string userId);
        Task<bool> MarkAsReadAsync(Guid id);
        Task<bool> MarkAllAsReadAsync(string userId);
        Task<int> GetUnreadCountAsync(string userId);
    }
}
