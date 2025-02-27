using System.ComponentModel.DataAnnotations;

namespace pizza_app.DTO.PizzaDTO
{
    public class PizzaCreateDto
    {
        [Required(ErrorMessage = "Le nom de la pizza est obligatoire.")]
        [MinLength(3, ErrorMessage = "Le nom de la pizza doit comporter au moins 3 caractères.")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prix de la pizza est obligatoire.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à zéro.")]
        public decimal Prix { get; set; }
        [Required(ErrorMessage = "La liste des ingrédients est obligatoire.")]

        public List<int> IngredientsIds { get; set; }
    }
}
