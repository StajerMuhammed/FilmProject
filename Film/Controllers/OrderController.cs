using Film.DTOs.Movies;
using Film.Entity.DTOs.Order;
using Film.Entity.Enum.OrderStatus;
using Film.Entity.Models;
using Film.Service.Services.ServiceOrder;
using Film.Services.ServiceCategory;
using Film.Services.ServiceFilm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Film.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IFilmService _filmService;
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService, IFilmService filmService)
        {
            _orderService = orderService;
            _filmService = filmService;// Tüm kategorileri döndür
        }

        [HttpGet]
        public IActionResult GetOrder(string? search = null, int page = 1, int pageSize = 5)
        {
            var orders = _orderService.GetAllOrders();

            if (!string.IsNullOrEmpty(search))
            {
                orders = orders.Where(f => f.Film.Name.Replace(" ", "").ToLower().Contains(search.Replace(" ", "").ToLower())).ToList();
            }

            var totalRecords = orders.Count();
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

            var paginatedOrders = orders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var ordersDTO = paginatedOrders.Select(order => new OrderDTO
            {
                Id = order.Id,
                FilmId = order.FilmId,
                FilmName = order.Film!.Name,
                CategoryId = order.Film.CategoryId,
                KategoriTürü = order.Film.Kategori?.Tür,
                YönetmenId = order.Film.YönetmenId,
                YönetmenName = order.Film.Yönetmen?.Name,
                ImageUrl = order.Film.ImageUrl,
                Overview = order.Film.Overview,
                Rating = order.Film.Rating,
                Price = order.Film.Price,
                Status = OrderStatus.Sepette.ToString(),  // Varsayılan durum 'Sepette'
                CreatedDate = DateTime.Now,  // Siparişin oluşturulma tarihi,
            }).ToList();

            return Ok(new
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                Page = page,
                PageSize = pageSize,
                Data = ordersDTO
            });
        }


        [HttpGet("GetOrder/{id}")]
        public ActionResult<OrderDTO> GetOrder(int id)
        {
            var order = _orderService.GetOrderById(id);

            if (order == null)
                return NotFound();

            // Film nesnesinin null olup olmadığını kontrol et
            var filmName = order.Film != null ? order.Film.Name : "Film adı bulunamadı";

            var orderDTO = new OrderDTO
            {
                Id = order.Id,
                FilmId = order.FilmId,
                FilmName = order.Film!.Name,
                CategoryId = order.Film.CategoryId,
                KategoriTürü = order.Film.Kategori?.Tür,
                YönetmenId = order.Film.YönetmenId,
                YönetmenName = order.Film.Yönetmen?.Name,
                ImageUrl = order.Film.ImageUrl,
                Overview = order.Film.Overview,
                Rating = order.Film.Rating,
                Price = order.Film.Price,
                Status = OrderStatus.Sepette.ToString(),  // Varsayılan durum 'Sepette'
                CreatedDate = DateTime.Now,  // Siparişin oluşturulma tarihi,
            };

            return Ok(orderDTO);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] OrderForInsertion orderForInsertion)
        {
            if (orderForInsertion == null)
                return BadRequest("Geçersiz istek");

            // Siparişi oluşturuyoruz ve backend tarafında Status ve CreatedDate'yi belirliyoruz
            var order = _orderService.CreateOrder(orderForInsertion);

            // DTO'yu oluşturuyoruz

            // Film DTO oluşturuluyor
            var orderDto = new OrderDTO
            {
                Id = order.Id,
                FilmId = order.FilmId,
                FilmName = order.Film!.Name,
                CategoryId = order.Film.CategoryId,
                KategoriTürü = order.Film.Kategori?.Tür,
                YönetmenId = order.Film.YönetmenId,
                YönetmenName = order.Film.Yönetmen?.Name,
                ImageUrl = order.Film.ImageUrl,
                Overview = order.Film.Overview,
                Rating = order.Film.Rating,
                Price = order.Film.Price,
                Status = OrderStatus.Sepette.ToString(),  // Varsayılan durum 'Sepette'
                CreatedDate = DateTime.Now,  // Siparişin oluşturulma tarihi,
            };

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, orderDto); // Yeni film oluşturulunca 201 döndür
        }



        [HttpPut("UpdateOrder/{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] OrderForUpdate orderForUpdate)
        {
            if (orderForUpdate == null || orderForUpdate.Id != id)
                return BadRequest("ID uyuşmazlığı"); // ID uyuşmazlığı varsa 400 döndür

            var updatedOrder = _orderService.UpdateOrder(orderForUpdate);

            if (updatedOrder == null)
                return NotFound(); // Eğer güncellenen film bulunamadıysa 404 döndür

            var orderDTO = new OrderDTO
            {
                Id = updatedOrder.Id,
                FilmId = updatedOrder.FilmId,
                FilmName = updatedOrder.Film!.Name,
                CategoryId = updatedOrder.Film.CategoryId,
                KategoriTürü = updatedOrder.Film.Kategori?.Tür,
                YönetmenId = updatedOrder.Film.YönetmenId,
                YönetmenName = updatedOrder.Film.Yönetmen?.Name,
                ImageUrl = updatedOrder.Film.ImageUrl,
                Overview = updatedOrder.Film.Overview,
                Rating = updatedOrder.Film.Rating,
                Price = updatedOrder.Film.Price,
                Status = OrderStatus.Sepette.ToString(),  // Varsayılan durum 'Sepette'
                CreatedDate = DateTime.Now,  // Siparişin oluşturulma tarihi,
            };

            return Ok(orderDTO); // Güncelleme başarılıysa 200 döndür
        }



       
        // DELETE: api/Film/{id}
        [HttpDelete("DeleteOrder/{id}")]
        public ActionResult DeleteFilm(int id)
        {
            _orderService.DeleteOrder(id);
            return NoContent(); // İşlem başarılıysa 204 döner
        }
    }
}
