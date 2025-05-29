namespace EcoRoute.DataCollection.Dtos
{
    public class CreateNotificationDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } = "Info";
        public string UserId { get; set; } // Belirli kullanıcıya gidecekse
        public string UserRole { get; set; } = ""; // Rol grubu (ör: "Manager")
        public string Url { get; set; } // Bildirim tıklanınca yönlendirme

    }
}
