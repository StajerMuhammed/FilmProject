using Film.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Film.Service.Services.ServiceUser
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers(); // Tüm User DTO'larını getir
        User GetUserById(int id); // ID'ye göre User Getir
        User CreateUser(Film.Entity.DTOs.User.UserForInsertion userForInsertion); // Yeni User oluşturur
        User UpdateUser(Film.Entity.DTOs.User.UserForUpdate userForUpdate); // User güncelle
        void DeleteUser(int id); // User sil
    }
}
