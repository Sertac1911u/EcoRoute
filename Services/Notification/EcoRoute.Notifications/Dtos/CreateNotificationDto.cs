namespace EcoRoute.Notifications.Dtos
{
    public class CreateNotificationDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } = "Info"; // Default to Info
        public string UserId { get; set; } // Null for all users
        public string UserRole { get; set; } = ""; // Eklenmesi gereken satır

        public string Url { get; set; }

    }
}
