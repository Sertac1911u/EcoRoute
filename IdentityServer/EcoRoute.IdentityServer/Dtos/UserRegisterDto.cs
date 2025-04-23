namespace EcoRoute.IdentityServer.Dtos
{
    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Number { get; set; }
        public string Role { get; set; } // "SuperAdmin", "Manager", "Driver"

    }
}
