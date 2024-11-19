using Film.DTOs.Movies;
using Film.DTOs.Yönetmen;
using Film.Models;
using Film.Services.ServiceYonetmen;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Film.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YonetmenController : ControllerBase
    {
        private readonly IYonetmenService _yonetmenService;


        public YonetmenController(IYonetmenService yonetmenService)
        {
            _yonetmenService = yonetmenService;
        }

        // GET: api/Yonetmen
        [HttpGet]
        public IActionResult GetAllYonetmen(int page = 1, int pageSize = 5)
        {
            var yonetmenler = _yonetmenService.GetAllYonetmen();

            var totalRecords = yonetmenler.Count();
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

            var paginatedYonetmenler = yonetmenler
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(yonetmen => new YönetmenDTO
                {
                    BirtDay = yonetmen.BirtDay,
                    Id = yonetmen.Id,
                    Name = yonetmen.Name,
                    FilmAdları = yonetmen.Filmler.Select(f => f.Name).ToList()
                }).ToList();

            return Ok(new
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                Page = page,
                PageSize = pageSize,
                Data = paginatedYonetmenler
            });
        }




        [HttpGet("[action]/{id}")]
        public IActionResult GetYönetmen(int id)
        {
            var yonetmen = _yonetmenService.GetByID(id);

            if (yonetmen == null)
                return NotFound(); // Yönetmen bulunamazsa 404 döndür

            var yonetmenDTO = new YönetmenDTO
            {
                Id = yonetmen.Id,
                Name = yonetmen.Name,
                BirtDay = yonetmen.BirtDay,
                FilmAdları = yonetmen.Filmler?.Select(f => f.Name).ToList() ?? new List<string>() // Film adlarını ekle, null kontrolü
            };

            return Ok(yonetmenDTO); // YönetmenDTO döndür
        }


        [HttpPost("[action]")]
        public ActionResult<Yönetmen> CreateYonetmen([FromBody] YönetmenForInsertion yönetmenForInsertion)
        {
            if (yönetmenForInsertion == null)
                return BadRequest(); // Geçersiz istek

            var yonetmen = _yonetmenService.CreatYonetmen(yönetmenForInsertion);

            var yonetmenDto = new YönetmenDTO
            {
                Id = yonetmen.Id,
                Name = yonetmen.Name,
                BirtDay = yonetmen.BirtDay,
                FilmAdları = yonetmen.Filmler.Select(f => f.Name).ToList()
            };

            return Ok(yonetmenDto); // Yeni kategori oluşturulunca 201 döndür
        }


        [HttpPut("[action]")]
        public ActionResult UpdateYonetmen([FromBody] YönetmenForUpdate yönetmenForUpdate)
        {
            if (yönetmenForUpdate == null || yönetmenForUpdate.Id != yönetmenForUpdate.Id)
                return BadRequest();

            var updatedYonetmen = _yonetmenService.UpdateYonetmen(yönetmenForUpdate);

            var yonetmenDto = new YönetmenDTO
            {
                Id = updatedYonetmen.Id,
                Name = updatedYonetmen.Name,
                BirtDay = updatedYonetmen.BirtDay,
                FilmAdları = updatedYonetmen.Filmler.Select(f => f.Name).ToList()
            };

            return Ok(yonetmenDto); // Güncellenen DTO'yu döndür
        }



        // DELETE: api/Film/{id}
        [HttpDelete("[action]")]
        public ActionResult DeleteYonetmen(int id)
        {
            _yonetmenService.DeleteYönetmen(id);
            return NoContent(); // Silme işlemi başarılıysa 204 döndür
        }

    }
}
