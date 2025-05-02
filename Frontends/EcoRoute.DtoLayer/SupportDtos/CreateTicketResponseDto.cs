

namespace EcoRoute.DtoLayer.SupportDtos
{
    public class CreateTicketResponseDto
    {
        public Guid SupportTicketId { get; set; }
        public string Message { get; set; } = null!;
        public bool IsStaff { get; set; } = false;
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
