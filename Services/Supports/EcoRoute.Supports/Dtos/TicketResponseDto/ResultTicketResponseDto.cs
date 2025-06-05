namespace EcoRoute.Supports.Dtos.TicketResponseDto
{
    public class ResultTicketResponseDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = null!;
        public DateTime ResponseDate { get; set; }
        public bool IsStaff { get; set; }
        public string? AttachmentPath { get; set; }

        public string? UserId { get; set; }
        public string? UserName { get; set; }

    }
}
