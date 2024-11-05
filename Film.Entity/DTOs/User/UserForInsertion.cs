namespace Film.Entity.DTOs.User
{
    public class UserForInsertion
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
    }
}
