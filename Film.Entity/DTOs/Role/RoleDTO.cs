﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Film.Entity.DTOs.Role
{
        public class RoleDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<string> UserNames { get; set; } // Sadece kullanıcı isimleri
        }
}