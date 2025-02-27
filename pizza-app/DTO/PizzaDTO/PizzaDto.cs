using System.ComponentModel.DataAnnotations;

namespace pizza_app.DTO.PizzaDTO
{
    public class PizzaDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le nom de la pizza est obligatoire.")]
        [MinLength(3, ErrorMessage = "Le nom de la pizza doit comporter au moins 3 caractères.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prix de la pizza est obligatoire.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à zéro.")]
        public decimal Prix { get; set; }

        [Required(ErrorMessage = "La liste des ingrédients est obligatoire.")]
        [MinLength(1, ErrorMessage = "La pizza doit contenir au moins un ingrédient.")]
        public List<string> Ingredients { get; set; } = new List<string>(); // Liste des ingrédients

    }
}
