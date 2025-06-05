namespace EcoRoute.Supports.Entities
{
    public class SupportTicket
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } = null!;
        public string? Description { get; set; }
        public string Status { get; set; } = "Açık";
        public string Priority { get; set; } = "Orta";
        public string? Category { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string? AttachmentPath { get; set; }

        public string? UserId { get; set; }
        public string? UserName { get; set; }

        public virtual ICollection<TicketResponse> Responses { get; set; } = new List<TicketResponse>();
    }
}
