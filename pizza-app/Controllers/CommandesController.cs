using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pizza_app.DTO;
using pizza_app.DTO.Commandes;
using pizza_app.DTO.PizzaDTO;
using pizza_app.Enums;
using pizza_app.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace pizza_app.Controllers
{
    [ApiController]
    [Route("client/commandes")]
    public class CommandesController : ControllerBase
    {
        private readonly ICommandeService _commandeService;




        public CommandesController(ICommandeService commandeService)
        {
            _commandeService = commandeService;
         
        }

        // Passer une commande (Client)
        [Authorize] // Assurez-vous que l'utilisateur est connecté
        [HttpPost]


        [SwaggerOperation(Summary = "Creer une nouvelle Commande", Description = "Pour Creer une commande l'utilisateur doit etre authentifié!")]
        public async Task<IActionResult> CreateCommande([FromBody] CommandeCreateDto commandeCreateDto)
        {
            try
            {

                // Vérification de l'authentification
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new { message = "Utilisateur non authentifié" });
                }

                var clientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(clientId))
                {
                    return Unauthorized(new { message = "Impossible de récupérer l'identifiant de l'utilisateur." });
                }

                foreach (var commandePizzaDto in commandeCreateDto.CommandePizzas)
                {
                    if (commandePizzaDto.PizzaId <= 0)
                    {
                        return BadRequest(new { message = "L'ID de la pizza doit être valide." });
                    }
                }

                var commande = await _commandeService.PassCommandeAsync(commandeCreateDto, clientId);

                return CreatedAtAction(nameof(GetCommandeByIdForClient), new { id = commande.Id }, new
                {
                    message = "Commande créée avec succès",
                    commande = new
                    {
                        commande.Id,
                        commande.DateCommande,
                        Status = commande.Status.ToString()
                    }
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        // Suivre une commande (Client)
        // Suivre une commande (Client)
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Recuperer Commande avec l'id", Description = "l'utilisateur doit etre authentifié!")]
        public async Task<IActionResult> GetCommandeByIdForClient(int id)
        {
            var clientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(clientId))
            {
                return Unauthorized(new { message = "Utilisateur non authentifié." });
            }

            var commande = await _commandeService.GetCommandeByIdForClientAsync(id, clientId);

            if (commande == null)
            {
                return NotFound(new { message = "Commande non trouvée ou non autorisée." });
            }

            return Ok(commande);
        }

        // Liste des commandes à traiter (Admin)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Recuperer toutes les Commandes", Description = "Seuls les admins peuvent voir toutes les commandes")]
        public async Task<IActionResult> GetAllCommandes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var commandes = await _commandeService.GetAllCommandesAsync(pageNumber, pageSize);
            return Ok(commandes);
        }

        // Modifier le statut de la commande (Admin)
        [SwaggerOperation(Summary = "Modifier le status de la commande", Description = "Le statut doit être l'un des suivants : 'EnCours', 'Livree', 'EnChemin'. Seuls les admins peuvent modifier le status")]
        [HttpPut("/{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCommandeStatus(int id, [FromBody] CommandeStatusUpdateDto statusUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
            }

            if (!Enum.TryParse(statusUpdateDto.Status, true, out CommandeStatus status))
            {
                return BadRequest(new { message = "Le statut doit être l'un des suivants : 'EnCours', 'Livree', 'EnChemin'." });
            }

            var commande = await _commandeService.UpdateCommandeStatusAsync(id, status);
            if (commande == null)
            {
                return NotFound(new { message = "Commande non trouvée." });
            }

            return Ok(new
            {
                message = "Commande mise à jour avec succès",
                commande = new
                {
                    id = commande.Id,
                    status = commande.Status.ToString(),
                    dateCommande = commande.DateCommande
                }
            });
        }
    }


}
