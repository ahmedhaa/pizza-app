namespace pizza_app.Entities.Pizzas
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; } // Prix de l'ingrédient (optionnel)
        public ICollection<PizzaIngredient> PizzaIngredients { get; set; }




    }
}
