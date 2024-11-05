using System;
using System.Collections.Generic;

namespace Film.Entity.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        // Kullanıcı ve role arasında bir ilişki kurmak için gerekli
        public ICollection<User> Users { get; set; } = new List<User>();

        public bool IsDeleted { get; set; }
    }
}
