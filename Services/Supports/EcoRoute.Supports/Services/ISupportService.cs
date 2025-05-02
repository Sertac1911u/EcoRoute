using EcoRoute.Supports.Dtos.SupportTicketDto;
using EcoRoute.Supports.Dtos.TicketResponseDto;

namespace EcoRoute.Supports.Services
{
    public interface ISupportService
    {
        Task<ResultSupportTicketDto> CreateAsync(CreateSupportTicketDto dto);
        Task<List<ResultSupportTicketDto>> GetAllAsync();

        Task<List<ResultSupportTicketDto>> GetAllAsync(string userId, bool isManagerOrSuperAdmin);
        Task<GetByIdSupportTicketDto?> GetByIdAsync(Guid id);
        Task AddResponseAsync(CreateTicketResponseDto dto);
        Task<bool> UpdateStatusAsync(Guid id, string status);
        Task<bool> CloseTicketAsync(Guid id);
    }
}
