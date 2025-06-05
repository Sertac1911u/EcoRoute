namespace EcoRoute.Notifications.Dtos
{
    public class CreateNotificationDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } = "Info"; 
        public string UserId { get; set; }
        public string UserRole { get; set; } = ""; 

        public string Url { get; set; }

    }
}
