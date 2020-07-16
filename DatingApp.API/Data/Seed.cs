using System.Collections.Generic;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
{
    private readonly DataContext context;
    public Seed(DataContext _context)
    {
        context = _context;
    }

    public void SeedUsers()
    {
        var userData = System.IO.File.ReadAllText("Data/UserSeed.json");
        var users = JsonConvert.DeserializeObject<List<User>>(userData);

        foreach (var user in users)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(out passwordHash, out passwordSalt, "password");

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Name = user.Name.ToLower();


            context.Users.Add(user);
        }

        context.SaveChanges();
    }

    private void CreatePasswordHash(out byte[] passwordHash, out byte[] passwordSalt, string password)
    {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
    }
}
}
