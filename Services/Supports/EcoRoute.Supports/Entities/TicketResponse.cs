namespace EcoRoute.Supports.Entities
{
    public class TicketResponse
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = null!;
        public DateTime ResponseDate { get; set; } = DateTime.Now;
        public bool IsStaff { get; set; }
        public string? AttachmentPath { get; set; }

        public string? UserId { get; set; }
        public string? UserName { get; set; }

        public Guid SupportTicketId { get; set; }

        public virtual SupportTicket SupportTicket { get; set; } = null!;
    }
}
