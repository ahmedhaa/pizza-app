using System.Text.Json.Serialization;

namespace pizza_app.Entities.Pizzas
{
    public class Pizza
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public decimal Prix { get; set; }
        [JsonIgnore]
        public ICollection<PizzaIngredient> PizzaIngredients { get; set; }
        [JsonIgnore]
        public ICollection<CommandePizza> CommandePizzas { get; set; } = new List<CommandePizza>();



    }
}
