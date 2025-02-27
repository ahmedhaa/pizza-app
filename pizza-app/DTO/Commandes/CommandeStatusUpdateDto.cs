using pizza_app.Enums;
using System.ComponentModel.DataAnnotations;

namespace pizza_app.DTO.Commandes
{
    public class CommandeStatusUpdateDto
    {
        [Required]
        [EnumDataType(typeof(CommandeStatus), ErrorMessage = "Le statut doit être l'un des suivants : 'EnCours', 'Livree', 'EnChemin'.")]
        public string Status { get; set; } //  "En cours", "Livrée", etc.

    }
}
