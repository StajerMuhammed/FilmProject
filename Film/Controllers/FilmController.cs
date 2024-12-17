using Film.DTOs.Movies;
using Film.Models;
using Film.Services.ServiceCategory;
using Film.Services.ServiceFilm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly IFilmService _filmService;
        private readonly ICategoryService _categoryService;

        public FilmController(IFilmService filmService, ICategoryService categoryService)
        {
            _filmService = filmService;// Tüm kategorileri döndür
            _categoryService = categoryService;
        }
        [HttpGet]
        public IActionResult GetFilms(string? search = null, int page = 1, int pageSize = 5)
        {
            var films = _filmService.GetAllFilms();

            if (!string.IsNullOrEmpty(search))
            {
                films = films.Where(f => f.Name.Replace(" ", "").ToLower().Contains(search.Replace(" ", "").ToLower())).ToList();
            }

            var totalRecords = films.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            if (page > totalPages)
            {
                return NotFound(new
                {
                    Message = "Belirtilen sayfada görüntülenecek veri yok.",
                    Page = page,
                    PageSize = pageSize,
                    TotalRecords = totalRecords
                });
            }

            var paginatedFilms = films
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var filmsDTO = paginatedFilms.Select(film => new FilmDTO
            {
                CategoryId = film.CategoryId,
                Id = film.Id,
                KategoriTürü = film.Kategori?.Tür ?? "Kategori Yok",
                YönetmenName = film.Yönetmen?.Name ?? "Yönetmen Yok",
                YönetmenId = film.Yönetmen?.Id ?? 0,
                Name = film.Name,
                Overview = film.Overview,
                Rating = film.Rating,
                ImageUrl = film.ImageUrl ?? "/images/default.jpg",
                Price = film.Price,
            }).ToList();

            return Ok(new
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                Page = page,
                PageSize = pageSize,
                Data = filmsDTO
            });
        }




        [HttpGet("[action]/{id}")]
        public IActionResult GetFilm(int id)
        {
            var film = _filmService.GetFilmById(id);

            if (film == null)
                return NotFound();

            var filmDTO = new FilmDTO
            {
                CategoryId = film.CategoryId,
                Id = film.Id,
                KategoriTürü = film.Kategori?.Tür ?? "Kategori Yok",  // Kategori null olabilir
                YönetmenName = film.Yönetmen?.Name ?? "Yönetmen Yok",  // Yönetmen null olabilir
                YönetmenId = film.Yönetmen?.Id ?? 0,  // Yönetmen null olabilir
                Name = film.Name,
                Overview = film.Overview,
                Rating = film.Rating,
                ImageUrl = film.ImageUrl ?? "/images/default.jpg",  // ImageUrl null ise varsayılan resim
                Price = film.Price,
            };

            return Ok(filmDTO); // Tek bir FilmDTO döndür
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<FilmDTO>> CreateFilm([FromBody] FilmForInsertion filmForInsertion)
        {
            if (filmForInsertion == null)
                return BadRequest("Geçersiz istek"); // Geçersiz istek

            // Resim URL'si yoksa hata ver
            if (string.IsNullOrEmpty(filmForInsertion.ImageUrl))
            {
                return BadRequest("Resim URL'si gerekli");
            }

            // Film oluşturuluyor
            var film = _filmService.CreateFilm(filmForInsertion);

            // Film DTO oluşturuluyor
            var filmDto = new FilmDTO
            {
                CategoryId = film.CategoryId,
                YönetmenId = film.YönetmenId,
                Id = film.Id,
                Name = film.Name,
                Overview = film.Overview,
                Rating = film.Rating,
                // Resim URL'si sağlandıysa ekliyoruz
                ImageUrl = film.ImageUrl,
                Price= film.Price,
                YönetmenName=film.Yönetmen?.Name,
                KategoriTürü=film.Kategori?.Tür
                
            };

            return CreatedAtAction(nameof(GetFilm), new { id = film.Id }, filmDto); // Yeni film oluşturulunca 201 döndür
        }

        [HttpPut("[action]/{id}")]
        public async Task<ActionResult> UpdateFilm(int id, [FromBody] FilmForUpdate filmForUpdate)
        {
            if (filmForUpdate == null || filmForUpdate.Id != id)
                return BadRequest("ID uyuşmazlığı"); // ID uyuşmazlığı varsa 400 döndür

            // Resim URL'si yoksa hata ver
            if (string.IsNullOrEmpty(filmForUpdate.ImageUrl))
            {
                return BadRequest("Resim URL'si gerekli");
            }

            var updatedFilm = _filmService.UpdateFilm(filmForUpdate);

            if (updatedFilm == null)
                return NotFound(); // Eğer güncellenen film bulunamadıysa 404 döndür

            var filmDTO = new FilmDTO
            {
                CategoryId = updatedFilm.CategoryId,
                Id = updatedFilm.Id,
                KategoriTürü = updatedFilm.Kategori?.Tür ?? "Kategori Yok",
                YönetmenName = updatedFilm.Yönetmen?.Name ?? "Yönetmen Yok",
                YönetmenId = updatedFilm.Yönetmen?.Id ?? 0,
                Name = updatedFilm.Name,
                Overview = updatedFilm.Overview,
                Rating = updatedFilm.Rating,
                Price = updatedFilm.Price,
                // Gerçek resim URL'si burada ekleniyor
                ImageUrl = updatedFilm.ImageUrl
            };

            return Ok(filmDTO); // Güncelleme başarılıysa 200 döndür
        }

        // DELETE: api/Film/{id}
        [HttpDelete("[action]/{id}")]
        public ActionResult DeleteFilm(int id)
        {
            _filmService.DeleteFilm(id);
            return NoContent(); // Silme işlemi başarılıysa 204 döndür
        }

    }
}