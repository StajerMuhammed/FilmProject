using Film.DTOs.Movies;
using Film.Entity.DTOs.User;
using Film.Entity.Models;
using Film.Service.Services.ServiceUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Film.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) 
        {
          _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult GetUsers(int page = 1, int pageSize = 10)
        {
            var users = _userService.GetAllUsers();

            var totalRecords = users.Count();
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

            var paginatedUsers = users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(user => new UserDTO
                {
                    Username = user.Username,
                    Id = user.Id,
                    Password = user.Password,
                    Email = user.Email,
                    Role = user.Role.Name,
                    RoleId = user.Role.Id,
                }).ToList();

            return Ok(new
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                Page = page,
                PageSize = pageSize,
                Data = paginatedUsers
            });
        }



        [HttpGet("[action]")]
        public IActionResult GetUser(int id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
                return NotFound();

            var userDTO = new UserDTO
            {
                Username = user.Username,
                Id = user.Id,
                Password = user.Password,
                Email = user.Email,
                Role = user.Role.Name,
                RoleId = user.Role.Id,
            };

            return Ok(userDTO); // Tek bir FilmDTO döndür
        }


        // POST: api/film
        [HttpPost("[action]")]
        public ActionResult<User> CreateUser([FromBody] UserForInsertion userForInsertion)
        {
            if (userForInsertion == null)
                return BadRequest(); // Geçersiz istek

            var user = _userService.CreateUser(userForInsertion);

            var userDto = new UserDTO
            {
                Username = user.Username,
                Id = user.Id,
                Password = user.Password,
                Email = user.Email,

                RoleId = user.RoleId,
            };

            return Ok(userDto); // Yeni kategori oluşturulunca 201 döndür
        }
        // PUT: api/Film/{id}
        [HttpPut("[action]")]
        public ActionResult UpdateUser([FromBody] UserForUpdate userForUpdate)
        {
            if (userForUpdate == null || userForUpdate.Id != userForUpdate.Id)
                return BadRequest(); // ID uyuşmazlığı varsa 400 döndür

            var updatedUser = _userService.UpdateUser(userForUpdate);
            return Ok(updatedUser); // Güncelleme başarılıysa 204 döndür
        }
        // DELETE: api/Film/{id}
        [HttpDelete("[action]")]
        public ActionResult DeleteUser(int id)
        {
            _userService.DeleteUser(id);
            return NoContent(); // Silme işlemi başarılıysa 204 döndür
        }
    }
}
