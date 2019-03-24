using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SN_BNB.Data
{
    public static class ApplicationSeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            //Create Roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Captain" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            //Create Users
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            if (userManager.FindByEmailAsync("admin1@outlook.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin1@outlook.com",
                    Email = "admin1@outlook.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "bnb").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
            if (userManager.FindByEmailAsync("captain1@outlook.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "captain1@outlook.com",
                    Email = "captain1@outlook.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "bnb").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Captain").Wait();
                }
            }
            if (userManager.FindByEmailAsync("player1@outlook.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "player1@outlook.com",
                    Email = "player1@outlook.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "bnb").Result;
                //Not in any role
            }
        }


    }
}
