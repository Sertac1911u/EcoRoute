namespace EcoRoute.Supports.Dtos.TicketResponseDto
{
    public class CreateTicketResponseDto
    {
        public Guid SupportTicketId { get; set; }
        public string Message { get; set; } = null!;
        public bool IsStaff { get; set; } = false;
        public IFormFile? Attachment { get; set; }

        // Kullanıcı bilgileri
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
