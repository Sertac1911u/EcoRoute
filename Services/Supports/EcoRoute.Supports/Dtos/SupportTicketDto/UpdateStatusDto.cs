namespace EcoRoute.Supports.Dtos.SupportTicketDto
{
    public class UpdateStatusDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = null!;
    }
}
