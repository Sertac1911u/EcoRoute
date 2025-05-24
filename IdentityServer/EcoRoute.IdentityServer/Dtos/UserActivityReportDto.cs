using System;

namespace EcoRoute.IdentityServer.Dtos
{
    public class UserActivityReportDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public int OperationCount { get; set; }

    }
}
