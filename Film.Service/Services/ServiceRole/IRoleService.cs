using Film.DTOs.Movies;
using Film.Entity.Models;
using Film.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Film.Service.Services.ServiceRole
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAllRoles(); // Tüm Role DTO'larını getir
        Role GetRoleById(int id); // ID'ye göre Role Getir
        Role CreateRole(Film.Entity.DTOs.Role.RoleForInsertion roleForInsertion); // Yeni Role oluşturur
        Role UpdateRole(Film.Entity.DTOs.Role.RoleForUpdate roleForUpdate); // Role güncelle
        void DeleteRole(int id); // Role sil
    }
}
