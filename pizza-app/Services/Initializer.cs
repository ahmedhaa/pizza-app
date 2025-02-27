using Microsoft.AspNetCore.Identity;
using pizza_app.Entities.UserModel;

namespace pizza_app.Services
{
    public class Initializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };

            // Créer les rôles si ils n'existent pas 
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Créer l'utilisateur Admin
            var user = await userManager.FindByEmailAsync("ahmed@test.com");
            if (user == null)
            {
                var adminUser = new User
                {
                    UserName = "ahmed",
                    Email = "ahmed@test.com"
                };

                var createUser = await userManager.CreateAsync(adminUser, "Password123!");
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    foreach (var error in createUser.Errors)
                    {
                        Console.WriteLine($"Erreur : {error.Code} - {error.Description}");
                    }
                }
            }
        }
    }
}
