using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pizza_app.Entities.UserModel;
using pizza_app.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace pizza_app.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Enregistre un nouvel utilisateur",
            Description = "Permet de créer un nouvel utilisateur avec un rôle spécifique. Les rôles disponibles sont 'Admin' et 'User'."
        )]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message, userId, role) = await _userService.RegisterAsync(model);

            if (!success)
                return BadRequest(new { Message = message });

            return Ok(new { Message = message, UserId = userId, Role = role });
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Supprime un utilisateur",
            Description = "Permet à un administrateur de supprimer un utilisateur existant à partir de son identifiant."
        )]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var (success, message) = await _userService.DeleteUserAsync(userId);

            if (!success)
                return NotFound(new { Message = message });

            return NoContent(); // 204 No Content (succès sans contenu)
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers()
        {
            var (success, message, users) = await _userService.GetAllUsersAsync();

            if (!success)
            {
                return BadRequest(message); // En cas d'erreur, retour d'un BadRequest avec le message d'erreur
            }

            return Ok(users); // Retour de la liste des utilisateurs
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _userService.GetAllRolesAsync();
            if (result.Success)
            {
                return Ok(result.Roles);
            }
            return BadRequest(result.Message);
        }

    }

}
