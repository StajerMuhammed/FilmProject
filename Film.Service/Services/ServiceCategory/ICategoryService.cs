// Services/ICategoryService.cs
// Services/ICategoryService.cs
using Film.DTOs.Category;
using Film.Models;
using System.Collections.Generic;

namespace Film.Services.ServiceCategory
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories(); // Tüm kategorileri Getir
        Category GetCategoryById(int id); // ID'ye göre kategori Getir
        Category CreateCategory(CategoryForInsertion category); // Yeni kategori oluşturur
        Category UpdateCategory(CategoryForUpdate category); // Kategoriyi güncelle
        void DeleteCategory(int id); // Kategoriyi sil
    }
}