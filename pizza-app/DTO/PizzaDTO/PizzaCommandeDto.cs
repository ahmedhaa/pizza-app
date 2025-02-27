using System.ComponentModel.DataAnnotations;

namespace pizza_app.DTO.PizzaDTO
{
    public class PizzaCommandeDto
    {
        [Required(ErrorMessage = "Le nom de la pizza est requis.")]
        [StringLength(100, ErrorMessage = "Le nom de la pizza ne peut pas dépasser 100 caractères.")]
        public string PizzaNom { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être au moins 1.")]

        public int Quantite { get; set; }
    }
}
