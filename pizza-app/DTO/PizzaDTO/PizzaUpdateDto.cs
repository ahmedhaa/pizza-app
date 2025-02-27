namespace pizza_app.DTO.PizzaDTO
{
    public class PizzaUpdateDto
    {
        public string Nom { get; set; } = string.Empty;
        public decimal Prix { get; set; }
        public ICollection<int> IngredientsIds { get; set; } = new List<int>();
    }
}
