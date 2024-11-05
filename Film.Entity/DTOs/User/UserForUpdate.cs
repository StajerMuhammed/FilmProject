using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Film.Entity.DTOs.User
{
    public class UserForUpdate
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
