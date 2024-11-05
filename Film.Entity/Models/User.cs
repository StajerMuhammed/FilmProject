using System;
using System.Collections.Generic;

namespace Film.Entity.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        // Rol bilgisi için referans
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public bool IsDeleted { get; set; }
    }
}
