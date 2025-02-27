namespace pizza_app.DTO.Commandes
{
    public class CommandeCreateDto
    {
        public List<CommandePizzaCreateDto> CommandePizzas { get; set; } // Liste des pizzas avec leur quantité
    }
}
