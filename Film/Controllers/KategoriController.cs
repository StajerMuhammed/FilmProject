using Film.DTOs.Category;
using Film.Models;
using Film.Services.ServiceCategory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService; // Dependency Injection ile servis alınıyor
        }

        // GET: api/category
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            return Ok(_categoryService.GetAllCategories()); // Tüm kategorileri döndür
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        public ActionResult<Category> GetCategory(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound(); // Kategori bulunamazsa 404 döndür
            return Ok(category); // Kategori bulunduysa döndür
        }

        // POST: api/category
        [HttpPost]
        public ActionResult<Category> CreateCategory([FromBody] CategoryForInsertion categoryForInsertion)
        {
            if (categoryForInsertion == null)
                return BadRequest(); // Geçersiz istek

           var category = _categoryService.CreateCategory(categoryForInsertion);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category); // Yeni kategori oluşturulunca 201 döndür
        }

        // PUT: api/category/{id}
        [HttpPut]
        public ActionResult UpdateCategory( [FromBody] CategoryForUpdate categoryForUpdate)
        {
            if (categoryForUpdate == null || categoryForUpdate.Id != categoryForUpdate.Id)
                return BadRequest(); // ID uyuşmazlığı varsa 400 döndür

           var updatedCategory = _categoryService.UpdateCategory(categoryForUpdate);
            return Ok (updatedCategory); // Güncelleme başarılıysa 204 döndür
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCategory(int id)
        {
            _categoryService.DeleteCategory(id);
            return NoContent(); // Silme işlemi başarılıysa 204 döndür
        }
    }
}