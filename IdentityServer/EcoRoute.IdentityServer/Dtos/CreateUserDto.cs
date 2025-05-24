using System;

namespace EcoRoute.IdentityServer.Dtos
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.Now;
    }
}
