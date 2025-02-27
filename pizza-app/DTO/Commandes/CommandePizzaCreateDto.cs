using System.ComponentModel.DataAnnotations;

namespace pizza_app.DTO.Commandes
{
    public class CommandePizzaCreateDto
    {
        public int PizzaId { get; set; }  // Identifiant de la pizza

        [Required(ErrorMessage = "La quantité est obligatoire.")]
        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être un nombre supérieur ou égal à 1.")]
        public int Quantite { get; set; } // Quantité de la pizza commandée
    }
}
