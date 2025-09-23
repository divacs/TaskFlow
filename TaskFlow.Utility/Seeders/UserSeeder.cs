using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Models.Models;

namespace TaskFlow.Utility.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedAdmin(UserManager<ApplicationUser> userManager)
        {
            string adminEmail = "admin@taskflow.com";
            string password = "Admin123!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FullName = "Admin User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }
        }
    }
}
