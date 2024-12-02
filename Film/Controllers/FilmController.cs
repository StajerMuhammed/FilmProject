using Film.DTOs.Movies;
using Film.Models;
using Film.Services.ServiceCategory;
using Film.Services.ServiceFilm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        // GET: api/Film
        [HttpGet]
        public IActionResult GetFilms(int page = 1, int pageSize = 5)
        {
            var films = _filmService.GetAllFilms();

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
                KategoriTürü = film.Kategori!.Tür,
                YönetmenName = film.Yönetmen.Name,
                YönetmenId = film.Yönetmen.Id,
                Name = film.Name,
                Overview = film.Overview,
                Rating = film.Rating
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




        [HttpGet("[action]")]
        public IActionResult GetFilm(int id)
        {
            var film = _filmService.GetFilmById(id);

            if (film == null)
                return NotFound();

            var filmDTO = new FilmDTO
            {
                CategoryId = film.CategoryId,
                Id = film.Id,
                KategoriTürü = film.Kategori?.Tür,  // Kategori null olabilir
                YönetmenName = film.Yönetmen?.Name,  // Yönetmen null olabilir
                YönetmenId = film.Yönetmen?.Id ?? 0,  // Yönetmen null olabilir
                Name = film.Name,
                Overview = film.Overview,
                Rating = film.Rating,
            };

            return Ok(filmDTO); // Tek bir FilmDTO döndür
        }


        // POST: api/film
        [HttpPost("[action]")]
        public  ActionResult<Models.FilmModel> CreateFilm([FromBody] FilmForInsertion filmForInsertion)
        {
            if (filmForInsertion == null)
                return BadRequest(); // Geçersiz istek

            var film = _filmService.CreateFilm(filmForInsertion);

            var filmDto = new FilmDTO
            {
                CategoryId = film.CategoryId,
                YönetmenId = film.YönetmenId,

                Id = film.Id,
                Name = film.Name,
                Overview = film.Overview,
                Rating = film.Rating
            };

            return Ok(filmDto); // Yeni kategori oluşturulunca 201 döndür
        }
        // PUT: api/Film/{id}
        [HttpPut("[action]")]
        public ActionResult UpdateFilm([FromBody] FilmForUpdate filmForUpdate)
        {
            if (filmForUpdate == null || filmForUpdate.Id != filmForUpdate.Id)
                return BadRequest(); // ID uyuşmazlığı varsa 400 döndür

            var updatedFilm = _filmService.UpdateFilm(filmForUpdate);
            return Ok(updatedFilm); // Güncelleme başarılıysa 204 döndür
        }
        // DELETE: api/Film/{id}
        [HttpDelete("[action]")]
        public ActionResult DeleteFilm(int id)
        {
            _filmService.DeleteFilm(id);
            return NoContent(); // Silme işlemi başarılıysa 204 döndür
        }

    }
}
