using pizza_app.DTO.Commandes;
using pizza_app.Entities.Pizzas;
using pizza_app.Enums;
using pizza_app.Services.Common;

namespace pizza_app.Interfaces
{
    public interface ICommandeService
    {
        // Méthode pour passer une commande
        Task<Commande> PassCommandeAsync(CommandeCreateDto commandeDto, string clientId);

        // Méthode pour récupérer une commande par son ID
        Task<CommandeDto> GetCommandeByIdForClientAsync(int id, string clientId);
        // Méthode pour récupérer toutes les commandes
        Task<PagedResult<CommandeDto>> GetAllCommandesAsync(int pageNumber, int pageSize);

        // Méthode pour mettre à jour le statut d'une commande
        Task<Commande> UpdateCommandeStatusAsync(int id, CommandeStatus status);
    }
}
