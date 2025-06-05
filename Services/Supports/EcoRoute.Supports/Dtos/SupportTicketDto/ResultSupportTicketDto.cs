using EcoRoute.Supports.Dtos.TicketResponseDto;

namespace EcoRoute.Supports.Dtos.SupportTicketDto
{
    public class ResultSupportTicketDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } = null!;
        public string? Description { get; set; }
        public string Status { get; set; } = "Açık";
        public string Priority { get; set; } = "Orta";
        public string? Category { get; set; }
        public DateTime CreateDate { get; set; }
        public string? AttachmentPath { get; set; }

        public string? UserId { get; set; }
        public string? UserName { get; set; }

        public List<ResultTicketResponseDto> Responses { get; set; } = new();
    }
}
