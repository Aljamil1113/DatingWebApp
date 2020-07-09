using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;

        public AuthController(IAuthRepository _authRepository)
        {
            authRepository = _authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegister toRegister)
        {
            toRegister.Name = toRegister.Name.ToLower(); 

            if(await authRepository.IsUserExist(toRegister.Name))
            {
                return BadRequest("Username already exists");
            }

            var UserToCreate = new User{
                Name = toRegister.Name
            } ;

            var createdUser = await authRepository.Register(UserToCreate, toRegister.Password);

            return StatusCode(201);
        }
    }
}