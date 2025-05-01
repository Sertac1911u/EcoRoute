namespace EcoRoute.Supports.Dtos.SupportTicketDto
{
    public class CreateSupportTicketDto
    {
        public string Subject { get; set; } = null!;
        public string? Description { get; set; }
        public string Priority { get; set; } = "Orta";
        public string? Category { get; set; }
        public IFormFile? Attachment { get; set; }
    }
}
