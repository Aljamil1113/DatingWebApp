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
        public Seed(UserManager<User> _userManager)
        {
            userManager = _userManager;
        }

        public void SeedUsers()
        {
            if(!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeed.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach (var user in users)
            {
                userManager.CreateAsync(user, "password").Wait();
                // byte[] passwordHash, passwordSalt;
                // CreatePasswordHash(out passwordHash, out passwordSalt, "password");

                // user.PasswordHash = passwordHash;
                // user.PasswordSalt = passwordSalt;
               // user.UserName = user.UserName.ToLower();


                //context.Users.Add(user);
            }
            //context.SaveChanges();
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
