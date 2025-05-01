namespace EcoRoute.Supports.Entities
{
    public class TicketResponse
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = null!;
        public DateTime ResponseDate { get; set; } = DateTime.UtcNow;
        public bool IsStaff { get; set; } = false;
        public string? AttachmentPath { get; set; }

        public Guid SupportTicketId { get; set; }
        public SupportTicket SupportTicket { get; set; } = null!;
    }
}
