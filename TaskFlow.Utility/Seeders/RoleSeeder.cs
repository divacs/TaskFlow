using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Utility.Seeders
{
    public static class RoleSeeder
    {
        // this method seeds roles into the database if they do not already exist
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Administrator", "ProjectManager", "User" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
