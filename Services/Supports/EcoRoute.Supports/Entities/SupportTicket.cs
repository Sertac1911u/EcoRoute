namespace EcoRoute.Supports.Entities
{
    public class SupportTicket
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } = null!;
        public string? Description { get; set; }
        public string Status { get; set; } = "Açık"; // Açık, İşlemde, Çözüldü, Kapatıldı
        public string Priority { get; set; } = "Orta"; // Düşük, Orta, Yüksek, Acil
        public string? Category { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string? AttachmentPath { get; set; }

        public List<TicketResponse> Responses { get; set; } = new();
    }
}
