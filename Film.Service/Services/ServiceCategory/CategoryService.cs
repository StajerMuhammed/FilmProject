using Film.Datas; // DbContext sınıfı
using Film.DTOs.Category;
using Film.Models; // Modeller
using System.Collections.Generic;
using System.Linq;

namespace Film.Services.ServiceCategory
{
    public class CategoryService : ICategoryService
    {
        private readonly SampleDBContext _context;

        public CategoryService(SampleDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories() // Metot adını güncelledik
        {
            return _context.Categories.Where(c => !c.IsDeleted).ToList(); // Silinmemiş tüm kategorileri döndür
        }

        public Category GetCategoryById(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id && !c.IsDeleted); // Silinmemiş kategori bul

            if (category == null)
            {
                throw new Exception("Kategori bulunamadı");
            }
                
                return category;
        }

        public Category CreateCategory(CategoryForInsertion categoryForInsertion)
        {

            if (string.IsNullOrWhiteSpace(categoryForInsertion.Tür))
            {
                throw new ArgumentException("Tür alanı zorunludur."); // Hata kontrolü
            }
            var category = new Category
            {
                Tür = categoryForInsertion.Tür,
            };

            _context.Categories.Add(category); // Yeni kategori ekle
            _context.SaveChanges(); // Değişiklikleri kaydet

            return category;
        }

        public Category UpdateCategory(CategoryForUpdate categoryForUpdate)
        {
            var existingCategory = GetCategoryById(categoryForUpdate.Id);


            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Kategori ID {categoryForUpdate.Id} bulunamadı."); // Hata kontrolü
            }



            existingCategory.Tür = categoryForUpdate.Tür;
            existingCategory.IsDeleted = categoryForUpdate.IsDeleted;// Tür güncelle


            _context.SaveChanges(); // Değişiklikleri kaydet

            return existingCategory; // Güncellenen kategoriyi döndür
        }

        public void DeleteCategory(int id)
        {
            var category = GetCategoryById(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Kategori ID {id} bulunamadı."); // Hata kontrolü
            }

            category.IsDeleted = true; // Silinmiş olarak işaretle
            _context.SaveChanges(); // Değişiklikleri kaydet
        }
    }
}
