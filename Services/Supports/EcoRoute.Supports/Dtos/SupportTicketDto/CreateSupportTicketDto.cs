namespace EcoRoute.Supports.Dtos.SupportTicketDto
{
    public class CreateSupportTicketDto
    {
        public string Subject { get; set; } = null!;
        public string? Description { get; set; }
        public string Priority { get; set; } = "Orta";
        public string? Category { get; set; }
        public IFormFile? Attachment { get; set; }

        // Kullanıcı bilgileri
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
