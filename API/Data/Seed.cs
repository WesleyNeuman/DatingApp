using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
//using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return; // Exits the function if the sqlite database already has data

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<AppUser>>(userData); // Create a list of appusers from the seed data
            foreach (var user in users) // Artificially generate login data for the user
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                context.Users.Add(user); // Adds user to the data context, but DOESN'T put it in the database yet
            }

            await context.SaveChangesAsync(); // In this case, commits the added users to the database
        }
    }
}