using System;

namespace EcoRoute.IdentityServer.Dtos
{
    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? LastLoginDate { get; set; } = DateTime.Now;
    }
}
