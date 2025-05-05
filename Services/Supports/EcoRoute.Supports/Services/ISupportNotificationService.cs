using EcoRoute.Supports.Dtos.SupportTicketDto;
using EcoRoute.Supports.Dtos.TicketResponseDto;

namespace EcoRoute.Supports.Services
{
    public interface ISupportNotificationService
    {
        Task SendTicketCreatedNotificationAsync(ResultSupportTicketDto ticket);
        Task SendTicketResponseNotificationAsync(ResultTicketResponseDto response, string ticketSubject, Guid ticketId, string recipientUserId);
        Task SendTicketStatusChangedNotificationAsync(string ticketSubject, Guid ticketId, string status, string recipientUserId);
    }
}
