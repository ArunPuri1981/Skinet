using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Bob",
                    Email = "bob@gmail.com",
                    UserName = "bob@gmail.com",
                    Address = new Address
                    {
                        FirstName = "Bob",
                        LastName = "Smith",
                        Street = "123 Main Street",
                        City = "London",
                        State = "London",
                        ZipCode = 123456
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}