using Film.DTOs.Category;
using Film.Entity.DTOs.Role;
using Film.Entity.Models;
using Film.Models;
using Film.Service.Services.ServiceRole;
using Film.Services.ServiceCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Film.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService; // Dependency Injection ile servis alınıyor
        }

        // GET: api/Role
        [HttpGet]
        public IActionResult GetAllRoles(int page = 1, int pageSize = 2)
        {
            var roles = _roleService.GetAllRoles();

            var totalRecords = roles.Count();
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

            var paginatedRoles = roles
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(role => new
                {
                    role.Id,
                    role.Name,
                    Users = role.Users.Select(user => new
                    {
                        user.Id,
                        user.Username
                    }).ToList()
                }).ToList();

            return Ok(new
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                Page = page,
                PageSize = pageSize,
                Data = paginatedRoles
            });
        }




        [HttpGet("{id}")]
        public ActionResult GetRoleById(int id)
        {
            var role = _roleService.GetRoleById(id);

            if (role == null)
            {
                return NotFound("Role bulunamadı");
            }

            var result = new
            {
                role.Id,
                role.Name,
                Users = role.Users.Select(user => new
                {
                    user.Id,
                    user.Username
                }).ToList() // Kullanıcıların sadece Id ve Username bilgisi
            };

            return Ok(result); // Rol bilgisi ve kullanıcı Id ve Username bilgileriyle döndür
        }


        // POST: api/category
        [HttpPost]
        public ActionResult<Role> CreateRole([FromBody] RoleForInsertion roleForInsertion)
        {
            if (roleForInsertion == null)
                return BadRequest(); // Geçersiz istek

            var role = _roleService.CreateRole(roleForInsertion);
            return CreatedAtAction(nameof(GetAllRoles), new { id = role.Id }, role); // Yeni kategori oluşturulunca 201 döndür
        }

        // PUT: api/category/{id}
        [HttpPut]
        public ActionResult UpdateRole([FromBody] RoleForUpdate roleForUpdate)
        {
            if (roleForUpdate == null || roleForUpdate.Id != roleForUpdate.Id)
                return BadRequest(); // ID uyuşmazlığı varsa 400 döndür

            var updatedRole = _roleService.UpdateRole(roleForUpdate);
            return Ok(updatedRole); // Güncelleme başarılıysa 204 döndür
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteRole(int id)
        {
            _roleService.DeleteRole(id);
            return NoContent(); // Silme işlemi başarılıysa 204 döndür
        }
    }
}
