namespace EcoRoute.IdentityServer.Dtos
{
    public class UpdateUserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public string Role { get; set; } 
        public string Password { get; set; }
    }
}
