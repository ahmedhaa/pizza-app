using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using pizza_app.Entities.UserModel;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pizza_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Entities.UserModel.LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { message = "Email ou mot de passe incorrect." });

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // Génération du jeton JWT
                var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

             new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

                // Ajout des rôles de l'utilisateur 
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var token = new JwtSecurityToken(
                   issuer: "YourIssuer",
                   audience: "YourAudience",
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized(new { message = "Email ou mot de passe incorrect." });
        }

     
        [HttpGet("me")]
        [SwaggerOperation(Summary = "Recuperer Informations de l'utilisateur connecté ", Description = "Recuperer Informations de l'utilisateur connecté")]

        public async Task<IActionResult> GetProfile()
        {
            // Récupérer l'ID de l'utilisateur authentifié à partir du token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Utilise "NameIdentifier" pour obtenir l'ID
            if (userId == null)
            {
                return Unauthorized(new { message = "Utilisateur non authentifié" });
            }

            // Trouver l'utilisateur dans la base de données
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Utilisateur non trouvé" });
            }

            // Créer une réponse avec les informations de l'utilisateur
            var userInfo = new
            {
                user.Id,
                user.UserName,
                user.Email,
              
                Roles = await _userManager.GetRolesAsync(user)
            };

            return Ok(userInfo);
        }
    }
}
