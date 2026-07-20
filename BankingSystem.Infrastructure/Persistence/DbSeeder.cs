using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAsync (RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
