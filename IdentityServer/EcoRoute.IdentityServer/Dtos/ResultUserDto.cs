using System.Collections.Generic;

namespace EcoRoute.IdentityServer.Dtos
{
    public class ResultUserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

        public string SelectedRole { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
