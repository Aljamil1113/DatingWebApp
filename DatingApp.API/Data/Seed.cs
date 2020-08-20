using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        // private readonly DataContext context;
        public UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        public Seed(UserManager<User> _userManager, RoleManager<Role> _roleManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;
        }

        public void SeedUsers()
        {
            if (!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeed.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>
                {
                    new Role{Name = "Member"},
                    new Role{Name = "Admin"},
                    new Role{Name = "Moderator"},
                    new Role{Name = "VIP"}
                };

                foreach(var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    userManager.CreateAsync(user, "password").Wait();
                    userManager.AddToRoleAsync(user, "Member").Wait();
                    // byte[] passwordHash, passwordSalt;
                    // CreatePasswordHash(out passwordHash, out passwordSalt, "password");

                    // user.PasswordHash = passwordHash;
                    // user.PasswordSalt = passwordSalt;
                    // user.UserName = user.UserName.ToLower();


                    //context.Users.Add(user);
                }
                //context.SaveChanges();
                var adminUser = new User
                {
                    UserName = "Admin"
                };

                IdentityResult result = userManager.CreateAsync(adminUser, "password").Result;

                if(result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"}).Wait();
                }
            }
        }

        // private void CreatePasswordHash(out byte[] passwordHash, out byte[] passwordSalt, string password)
        // {
        //     using (var hmac = new System.Security.Cryptography.HMACSHA512())
        //     {
        //         passwordSalt = hmac.Key;
        //         passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //     }
        // }
    }
}
