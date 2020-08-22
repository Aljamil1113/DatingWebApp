using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //private readonly AuthRepository authRepository;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AuthController(IConfiguration _config, IMapper _mapper, UserManager<User> _userManager,
                        SignInManager<User> _signInManager)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            mapper = _mapper;
            config = _config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto toRegister)
        {
            // toRegister.Name = toRegister.Name.ToLower();

            // if (await authRepository.IsUserExist(toRegister.Name))
            // {
            //     return BadRequest("Username already exists");
            // }

            var userToCreate = mapper.Map<User>(toRegister);

            // var createdUser = await authRepository.Register(UserToCreate, toRegister.Password);

            // var userToReturn = mapper.Map<UserForDetailedDto>(createdUser);

            // return CreatedAtRoute("GetUser", new { Controller = "Users", id = createdUser.Id }, userToReturn);

            var result = await userManager.CreateAsync(userToCreate, toRegister.Password);

            var userToReturn = mapper.Map<UserForDetailedDto>(userToCreate);

            if(result.Succeeded)
            {
                return CreatedAtRoute("GetUser", new { Controller = "Users", id = userToCreate.Id }, userToReturn);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLogIn)
        {
            // var userFromRepo = await authRepository.Login(userForLogIn.Name.ToLower(), userForLogIn.Password);

            // if (userFromRepo == null)
            // {
            //     return Unauthorized();
            // }
            //var user = mapper.Map<UserForListDto>(userFromRepo);

            var user = await userManager.FindByNameAsync(userForLogIn.Username);
            
            if(user == null)
            {
                return Unauthorized();
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, userForLogIn.Password, false);
            if(result.Succeeded)
            {
                var appUser = await userManager.Users.Include(p => p.Photos)
                .FirstOrDefaultAsync(u => u.NormalizedUserName == userForLogIn.Username.ToUpper());

                var userToReturn = mapper.Map<UserForListDto>(appUser);

                 return Ok(new
                {
                    token = GenerateJwtToken(appUser).Result,
                    user = userToReturn
                });
            }

            return Unauthorized();
           
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}