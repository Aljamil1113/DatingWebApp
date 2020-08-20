using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly DataContext context;
        public AdminController(DataContext _context)
        {
            context = _context;
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("userWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await (from user in context.Users orderby user.UserName
            select new {
                Id = user.Id,
                Username = user.UserName,
                Roles = (from userRole in user.UserRoles
                join role in context.Roles
                on userRole.RoleId equals role.Id select role.Name).ToList()
            }).ToListAsync();

            return Ok(userList);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photosForModeration")]
        public IActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this");
        }
    }
}