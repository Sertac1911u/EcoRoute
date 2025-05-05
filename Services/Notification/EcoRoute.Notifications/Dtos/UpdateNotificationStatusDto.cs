namespace EcoRoute.Notifications.Dtos
{
    public class UpdateNotificationStatusDto
    {
        public Guid Id { get; set; }
        public bool IsRead { get; set; }

    }
}
