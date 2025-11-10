using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Models;
using System;
using System.Linq;

namespace MvcMovie.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // ✅ Création du rôle Admin s’il n’existe pas
            if (!roleManager.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
            }

            // ✅ Création de l’utilisateur admin
            if (!userManager.Users.Any(u => u.UserName == "admin@mvc.com"))
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@mvc.com",
                    Email = "admin@mvc.com",
                    EmailConfirmed = true
                };

                userManager.CreateAsync(admin, "Admin123!").Wait();
                userManager.AddToRoleAsync(admin, "Admin").Wait();
            }
        }
    }
}
