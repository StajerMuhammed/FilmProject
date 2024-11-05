using Film.Datas;
using Film.DTOs.Movies;
using Film.Entity.DTOs.User;
using Film.Entity.Models;
using Film.Service.Services.ServiceRole;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Film.Service.Services.ServiceUser
{
    public class UserService : IUserService
    {
        private readonly SampleDBContext _context;

        public UserService(SampleDBContext context)
        {
            _context = context;
        }

        public User CreateUser(UserForInsertion userForInsertion)
        {
            if (string.IsNullOrWhiteSpace(userForInsertion.Username))
            {
                throw new ArgumentException("İsim alanı zorunludur.");
            }

            // E-posta adresinin @ işareti içermesi zorunludur
            if (!userForInsertion.Email.Contains("@"))
            {
                throw new ArgumentException("E-posta adresi geçersiz. '@' işareti gereklidir.");
            }

            // E-posta adresinin .com ile bitmesi zorunludur
            if (!userForInsertion.Email.EndsWith(".com", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("E-posta adresi geçersiz. '.com' ile bitmelidir.");
            }

            // Role nesnesini veritabanından çekin
            var role = _context.Role.FirstOrDefault(r => r.Id == userForInsertion.RoleId);
            if (role == null)
            {
                throw new ArgumentException("Geçersiz RoleId.");
            }

            var user = new User
            {
                Username = userForInsertion.Username,
                Password = userForInsertion.Password,
                Email = userForInsertion.Email,
                RoleId = userForInsertion.RoleId,
                Role = role, // Çekilen rol nesnesi atanıyor
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }



        public void DeleteUser(int id)
        {
            var user = GetUserById(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User ID {id} bulunamadı."); // Hata kontrolü
            }

            user.IsDeleted = true; // Silinmiş olarak işaretle
            _context.SaveChanges(); // Değişiklikleri kaydet
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users
                 .Include(f => f.Role) // Kategoriyi dahil et
                 .Where(c => !c.IsDeleted)
                 .ToList();
        }

        public User GetUserById(int id)
        {
            var user = _context.Users
                .Include(f => f.Role)  // Role ilişkisini dahil et
                .FirstOrDefault(f => f.Id == id && !f.IsDeleted);  // Silinmemiş Useri bul

            if (user == null)
            {
                throw new Exception("User bulunamadı");
            }

            return user;  // User olarak döndür
        }

        public User UpdateUser(UserForUpdate userForUpdate)
        {
            var existingUser = GetUserById(userForUpdate.Id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User ID {userForUpdate.Id} bulunamadı."); // Hata kontrolü
            }


            // E-posta adresinin @ işareti içermesi zorunludur
            if (!userForUpdate.Email.Contains("@"))
            {
                throw new ArgumentException("E-posta adresi geçersiz. '@' işareti gereklidir.");
            }

            // E-posta adresinin .com ile bitmesi zorunludur
            if (!userForUpdate.Email.EndsWith(".com", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("E-posta adresi geçersiz. '.com' ile bitmelidir.");
            }

            existingUser.Username = userForUpdate.Username;
            existingUser.Password = userForUpdate.Password;
            existingUser.Email = userForUpdate.Email;
            existingUser.RoleId = userForUpdate.RoleId;

            // Role nesnesini güncellenen RoleId ile çekiyoruz
            existingUser.Role = _context.Role.FirstOrDefault(r => r.Id == userForUpdate.RoleId);
            if (existingUser.Role == null)
            {
                throw new ArgumentException("Geçersiz RoleId.");
            }

            existingUser.IsDeleted = userForUpdate.IsDeleted;

            _context.SaveChanges(); // Değişiklikleri kaydet

            return existingUser; // Güncellenen kullanıcıyı döndür
        }

    }
}
