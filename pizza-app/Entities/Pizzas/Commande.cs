using pizza_app.Enums;

namespace pizza_app.Entities.Pizzas
{
    public class Commande
    {
        public int Id { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public DateTime DateCommande { get; set; } = DateTime.UtcNow;
        public CommandeStatus Status { get; set; }


        // Liste des pizzas commandées (relation Many-to-Many)
        public ICollection<CommandePizza> CommandePizzas { get; set; } = new List<CommandePizza>();
    }
}
