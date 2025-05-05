namespace EcoRoute.Notifications.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } 
        public string UserId { get; set; }
        public string UserRole { get; set; } = ""; // Eklenmesi gereken satır

        public string Url { get; set; } 
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ReadDate { get; set; }
    }
}
