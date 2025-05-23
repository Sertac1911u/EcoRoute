using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoRoute.DtoLayer.IdentityDtos
{
    public class ResultUserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

        public string SelectedRole { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        //
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginDate { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }

}
