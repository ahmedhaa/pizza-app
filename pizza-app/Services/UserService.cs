using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pizza_app.DTO.UserDto;
using pizza_app.Entities.UserModel;
using pizza_app.Interfaces;

namespace pizza_app.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<(bool Success, string Message, List<UserDTO> Users)> GetAllUsersAsync()
        {
            _logger.LogInformation("Récupération de tous les utilisateurs");

            // Récupérer la liste des utilisateurs de manière synchrone
            var users = await _userManager.Users.ToListAsync();

            if (users == null || !users.Any())
            {
                _logger.LogWarning("Aucun utilisateur trouvé");
                return (false, "Aucun utilisateur trouvé.", new List<UserDTO>());
            }

            // Initialiser une liste pour stocker les utilisateurs avec leurs rôles
            var userDtos = new List<UserDTO>();

            // Pour chaque utilisateur, récupérer son rôle de manière asynchrone
            foreach (var user in users)
            {
                // Récupérer les rôles associés à l'utilisateur
                var roles = await _userManager.GetRolesAsync(user);

                // Créer un UserDto pour cet utilisateur
                var userDto = new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() // Récupérer le premier rôle de l'utilisateur
                };

                // Ajouter le UserDto à la liste
                userDtos.Add(userDto);
            }

            _logger.LogInformation("Récupération réussie de {Count} utilisateurs", userDtos.Count);
            return (true, "Utilisateurs récupérés avec succès", userDtos);
        }

        public async Task<(bool Success, string Message, List<string> Roles)> GetAllRolesAsync()
        {
            _logger.LogInformation("Récupération de tous les rôles");

            // Récupérer tous les rôles existants
            var roles = await _roleManager.Roles.ToListAsync();

            if (roles == null || !roles.Any())
            {
                _logger.LogWarning("Aucun rôle trouvé");
                return (false, "Aucun rôle trouvé.", new List<string>());
            }

            // Extraire les noms des rôles
            var roleNames = roles.Select(role => role.Name).ToList();

            _logger.LogInformation("Récupération réussie de {Count} rôles", roleNames.Count);
            return (true, "Rôles récupérés avec succès", roleNames);
        }


        public async Task<(bool Success, string Message, string? UserId, string? Role)> RegisterAsync(RegisterModel model)
        {
            _logger.LogInformation("Démarrage de l'inscription pour l'utilisateur {Email}", model.Email);

            // Vérifier si l'utilisateur existe déjà
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("L'email {Email} est déjà utilisé", model.Email);
                return (false, "Cet email est déjà utilisé.", null, null);
            }

            // Vérifier si un rôle est fourni et valide
            if (string.IsNullOrWhiteSpace(model.Role) || (model.Role != "Admin" && model.Role != "User"))
            {
                _logger.LogWarning("Le rôle fourni est invalide : {Role}", model.Role);
                return (false, "Le rôle doit être 'Admin' ou 'User'.", null, null);
            }

            // Vérifier si le rôle existe dans la base
            var roleExists = await _roleManager.RoleExistsAsync(model.Role);
            if (!roleExists)
            {
                _logger.LogWarning("Le rôle '{Role}' n'existe pas", model.Role);
                return (false, $"Le rôle '{model.Role}' n'existe pas.", null, null);
            }

            // Création de l'utilisateur
            var user = new User { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                string errors = string.Join("; ", result.Errors.Select(e => e.Description));
                _logger.LogError("Échec de l'inscription de l'utilisateur {Email}: {Errors}", model.Email, errors);
                return (false, $"Échec de l'inscription : {errors}", null, null);
            }

            // Ajout du rôle
            var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user); // Suppression en cas d'échec d'ajout de rôle
                _logger.LogError("Échec de l'ajout du rôle {Role} pour l'utilisateur {Email}", model.Role, model.Email);
                return (false, "Échec de l'ajout du rôle.", null, null);
            }

            _logger.LogInformation("Utilisateur {Email} inscrit avec succès avec le rôle {Role}", model.Email, model.Role);
            return (true, "Utilisateur enregistré avec succès", user.Id, model.Role);
        }

        public async Task<(bool Success, string Message)> DeleteUserAsync(string userId)
        {
            _logger.LogInformation("Tentative de suppression de l'utilisateur {UserId}", userId);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Utilisateur avec l'ID {UserId} non trouvé", userId);
                return (false, "Utilisateur non trouvé.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Échec de la suppression de l'utilisateur {UserId}", userId);
                return (false, "Échec de la suppression de l'utilisateur.");
            }

            _logger.LogInformation("Utilisateur {UserId} supprimé avec succès", userId);
            return (true, "Utilisateur supprimé avec succès.");
        }
    }
}
