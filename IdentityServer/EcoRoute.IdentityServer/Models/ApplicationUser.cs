using Microsoft.AspNetCore.Identity;
using System;

namespace EcoRoute.IdentityServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? LastLoginDate { get; set; }

    }
}
