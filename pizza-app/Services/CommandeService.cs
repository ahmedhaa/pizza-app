using Microsoft.EntityFrameworkCore;
using pizza_app.Data;
using pizza_app.DTO;
using pizza_app.DTO.Commandes;
using pizza_app.DTO.PizzaDTO;
using pizza_app.Entities.Pizzas;
using pizza_app.Enums;
using pizza_app.Interfaces;
using pizza_app.Services.Common;
using Microsoft.Extensions.Logging; // Ajouter l'espace de noms pour le logger

namespace pizza_app.Services
{
    public class CommandeService : ICommandeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPizzaService _pizzaService;
        private readonly ILogger<CommandeService> _logger; // Déclarer le logger

        // Injection du logger dans le constructeur
        public CommandeService(ApplicationDbContext context, IPizzaService pizzaService, ILogger<CommandeService> logger)
        {
            _context = context;
            _pizzaService = pizzaService;
            _logger = logger; // Initialiser le logger
        }

        public async Task<Commande> PassCommandeAsync(CommandeCreateDto commandeDto, string clientId)
        {
            if (commandeDto.CommandePizzas == null || !commandeDto.CommandePizzas.Any())
            {
                _logger.LogWarning("Tentative de passer une commande sans pizza. Client ID: {ClientId}", clientId);
                throw new ArgumentException("La commande doit contenir au moins une pizza.");
            }

            foreach (var commandePizzaDto in commandeDto.CommandePizzas)
            {
                // Vérifier que la pizza existe
                var pizza = await _pizzaService.GetPizzaByIdAsync(commandePizzaDto.PizzaId);
                if (pizza == null)
                {
                    _logger.LogWarning("Pizza avec ID {PizzaId} non trouvée. Client ID: {ClientId}", commandePizzaDto.PizzaId, clientId);
                    throw new ArgumentException($"La pizza avec l'ID {commandePizzaDto.PizzaId} n'existe pas.");
                }
            }

            var commande = new Commande
            {
                ClientId = clientId,  // Récupérer automatiquement l'ID du client connecté
                DateCommande = DateTime.UtcNow, // Date créée automatiquement
                Status = CommandeStatus.A_Traiter,
                CommandePizzas = commandeDto.CommandePizzas.Select(cp => new CommandePizza
                {
                    PizzaId = cp.PizzaId,
                    Quantite = cp.Quantite
                }).ToList()
            };

            _context.Commandes.Add(commande);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Commande passée avec succès. Commande ID: {CommandeId}, Client ID: {ClientId}", commande.Id, clientId);

            return commande;
        }

        public async Task<CommandeDto> GetCommandeByIdForClientAsync(int id, string clientId)
        {
            var commande = await _context.Commandes
                .Where(c => c.Id == id && c.ClientId == clientId) // Vérifie que la commande appartient au client
                .Include(c => c.CommandePizzas)
                .ThenInclude(cp => cp.Pizza)
                .FirstOrDefaultAsync();

            if (commande == null)
            {
                _logger.LogWarning("Commande non trouvée pour le client. Commande ID: {CommandeId}, Client ID: {ClientId}", id, clientId);
                return null; // La commande n'existe pas ou n'appartient pas au client
            }

            return new CommandeDto
            {
                Id = commande.Id,
                DateCommande = commande.DateCommande,
                Status = commande.Status.ToString(), // Convertir l'énumération en string
                CommandePizzas = commande.CommandePizzas.Select(cp => new PizzaCommandeDto
                {
                    PizzaNom = cp.Pizza.Nom,
                    Quantite = cp.Quantite
                }).ToList()
            };
        }

        public async Task<PagedResult<CommandeDto>> GetAllCommandesAsync(int pageNumber, int pageSize)
        {
            var query = _context.Commandes
               // .Where(c => c.Status == CommandeStatus.A_Traiter) // Filtre sur 'A_Traiter'
                .Include(c => c.CommandePizzas)
                .ThenInclude(cp => cp.Pizza)
                .AsQueryable();

            // Obtenir le nombre total de commandes filtrées
            int totalRecords = await query.CountAsync();

            // Appliquer la pagination
            var commandes = await query
                .OrderByDescending(c => c.DateCommande) // Trier par date
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = commandes.Select(c => new CommandeDto
            {
                Id = c.Id,
                DateCommande = c.DateCommande,
                Status = Enum.IsDefined(typeof(CommandeStatus), c.Status) ? c.Status.ToString() : "Statut inconnu",
                CommandePizzas = c.CommandePizzas.Select(cp => new PizzaCommandeDto
                {
                    PizzaNom = cp.Pizza.Nom,
                    Quantite = cp.Quantite
                }).ToList()
            }).ToList();

            _logger.LogInformation("Récupération des commandes avec succès. Total des commandes: {TotalRecords}, Page: {PageNumber}, Taille de la page: {PageSize}", totalRecords, pageNumber, pageSize);

            return new PagedResult<CommandeDto>(result, pageNumber, pageSize, totalRecords);
        }

        public async Task<Commande> UpdateCommandeStatusAsync(int id, CommandeStatus status)
        {
            var commande = await _context.Commandes.FirstOrDefaultAsync(c => c.Id == id);
            if (commande == null)
            {
                _logger.LogWarning("Commande non trouvée pour la mise à jour du statut. Commande ID: {CommandeId}", id);
                return null;
            }

            // Mise à jour du statut de la commande
            commande.Status = status;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Statut de la commande mis à jour. Commande ID: {CommandeId}, Nouveau statut: {Status}", id, status);

            return commande;
        }
    }
}
