using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TaskFlow.Models.Models;

namespace TaskFlow.Utility.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            // --- ADMIN ---
            string adminEmail = "admin@taskflow.com";
            string adminPassword = "Admin123!";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FullName = "Admin User",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }

            // --- PROJECT MANAGER ---
            string pmEmail = "pm@taskflow.com";
            string pmPassword = "Pm12345!";
            if (await userManager.FindByEmailAsync(pmEmail) == null)
            {
                var pmUser = new ApplicationUser
                {
                    UserName = "pm",
                    Email = pmEmail,
                    FullName = "Project Manager",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(pmUser, pmPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(pmUser, "ProjectManager");
                }
            }

            // --- DEVELOPER ---
            string devEmail = "dev@taskflow.com";
            string devPassword = "Dev12345!";
            if (await userManager.FindByEmailAsync(devEmail) == null)
            {
                var devUser = new ApplicationUser
                {
                    UserName = "developer",
                    Email = devEmail,
                    FullName = "Developer User",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(devUser, devPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(devUser, "Developer");
                }
            }
        }
    }
}
