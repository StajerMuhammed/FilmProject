using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Film.Entity.DTOs.Role
{
    public class RoleForUpdate
    {
        public int Id { get; set; } // Role ID
        public string Name { get; set; } = null!; // Role adı
        public bool IsDeleted { get; set; }
    }
}
