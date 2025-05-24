using Microsoft.AspNetCore.Identity;
using System;

namespace EcoRoute.IdentityServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? LastLoginDate { get; set; }

    }
}
