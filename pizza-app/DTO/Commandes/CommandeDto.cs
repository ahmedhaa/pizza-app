using pizza_app.DTO.PizzaDTO;
using System.ComponentModel.DataAnnotations;

namespace pizza_app.DTO.Commandes
{
    public class CommandeDto
    {
        public int Id { get; set; }

        public DateTime DateCommande { get; set; }

        [Required(ErrorMessage = "Le statut de la commande est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le statut de la commande ne peut pas dépasser 50 caractères.")]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "La commande doit contenir au moins une pizza.")]
        public List<PizzaCommandeDto> CommandePizzas { get; set; } = new List<PizzaCommandeDto>();
    }
}
