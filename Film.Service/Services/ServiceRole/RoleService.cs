using Film.Datas;
using Film.DTOs.Category;
using Film.Entity.DTOs.Role;
using Film.Entity.Models;
using Film.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Film.Service.Services.ServiceRole
{
    public class RoleService : IRoleService
    {
        private readonly SampleDBContext _context;

        public RoleService(SampleDBContext context)
        {
            _context = context;
        }

        public Role CreateRole(RoleForInsertion roleForInsertion)
        {
            if (string.IsNullOrWhiteSpace(roleForInsertion.Name))
            {
                throw new ArgumentException("Role alanı zorunludur."); // Hata kontrolü
            }
            
            var role = new Role
            {
                Name = roleForInsertion.Name,
            };

            _context.Role.Add(role); // Yeni kategori ekle
            _context.SaveChanges(); // Değişiklikleri kaydet

            return role;
        }

        public void DeleteRole(int id)
        {
            var role = GetRoleById(id);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role ID {id} bulunamadı."); // Hata kontrolü
            }

            role.IsDeleted = true; // Silinmiş olarak işaretle
            _context.SaveChanges(); // Değişiklikleri kaydet
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _context.Role
                .Where(c => !c.IsDeleted)
                .Include(r => r.Users) // İlişkili kullanıcıları dahil et
                .ToList();
        }

        public Role GetRoleById(int id)
        {
            var role = _context.Role
                .Include(r => r.Users)  // İlişkili kullanıcıları dahil et
                .FirstOrDefault(r => r.Id == id && !r.IsDeleted);

            if (role == null)
            {
                throw new Exception("Role bulunamadı");
            }

            return role;  // Role nesnesi olarak döndür
        }



        public Role UpdateRole(RoleForUpdate roleForUpdate)
        {
            var existingRole = GetRoleById(roleForUpdate.Id);


            if (existingRole == null)
            {
                throw new KeyNotFoundException($"Rol ID {roleForUpdate.Id} bulunamadı."); // Hata kontrolü
            }



            existingRole.Name = roleForUpdate.Name;
            existingRole.IsDeleted = roleForUpdate.IsDeleted;// Tür güncelle


            _context.SaveChanges(); // Değişiklikleri kaydet

            return existingRole; // Güncellenen kategoriyi döndür
        }
    }
}
