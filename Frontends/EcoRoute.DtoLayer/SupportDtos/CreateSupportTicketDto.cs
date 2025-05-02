
namespace EcoRoute.DtoLayer.SupportDtos
{
    public class CreateSupportTicketDto
    {
        public string Subject { get; set; } = null!;
        public string? Description { get; set; }
        public string Priority { get; set; } = "Orta";
        public string? Category { get; set; }

        public string? UserId { get; set; }
        public string? UserName { get; set; }

    }
}
