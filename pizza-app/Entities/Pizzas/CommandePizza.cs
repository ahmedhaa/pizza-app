namespace pizza_app.Entities.Pizzas
{
    public class CommandePizza
    {
        public int CommandeId { get; set; }
        public Commande Commande { get; set; } = null!;

        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; } = null!;

        public int Quantite { get; set; }  // Nombre de pizzas commandées
    }
}
