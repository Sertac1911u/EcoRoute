namespace EcoRoute.Reports.Dtos
{
    public class UserActivityReportDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public int OperationCount { get; set; }
    }
}
