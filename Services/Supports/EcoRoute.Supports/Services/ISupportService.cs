using EcoRoute.Supports.Dtos.SupportTicketDto;

namespace EcoRoute.Supports.Services
{
    public interface ISupportService
    {
        Task<Guid> CreateAsync(CreateSupportTicketDto dto);
        Task<List<ResultSupportTicketDto>> GetAllAsync();
        Task<GetByIdSupportTicketDto?> GetByIdAsync(Guid id);
    }
}
