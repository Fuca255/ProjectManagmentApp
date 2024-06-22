namespace server.DTOs.Users
{
    public class UserRegistrationDto
    {
        public required string Username { get; set; }
        public required string Name { get; set; }
        public required string Lastname { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }    
        public required int RoleId { get; set; }
        public string Department {get; set;}
        public string Organisation {get; set;}
    }
}
