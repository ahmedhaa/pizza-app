using pizza_app.DTO.PizzaDTO;
using pizza_app.Entities.Pizzas;
using pizza_app.Services.Common;

namespace pizza_app.Interfaces
{
    public interface IPizzaService
    {
        Task<Pizza> AddPizzaAsync(PizzaCreateDto pizzaCreateDto);
        Task<Pizza> UpdatePizzaAsync(int id, PizzaUpdateDto pizzaUpdateDto);
        Task<Pizza> GetPizzaByIdAsync(int id);
        Task<PagedResult<PizzaDto>> GetAllPizzasAsync(int pageNumber, int pageSize);
        Task DeletePizza(int id);
        Task<List<IngredientDto>> GetAllIngredientsAsync();
    }
}
