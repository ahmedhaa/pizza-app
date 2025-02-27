using Microsoft.EntityFrameworkCore;
using pizza_app.Data;
using pizza_app.DTO.PizzaDTO;
using pizza_app.Entities.Pizzas;
using pizza_app.Interfaces;
using pizza_app.Services.Common;
using Microsoft.Extensions.Logging;

namespace pizza_app.Services
{
    public class PizzaService : IPizzaService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PizzaService> _logger;

        public PizzaService(ApplicationDbContext context, ILogger<PizzaService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Pizza> AddPizzaAsync(PizzaCreateDto pizzaCreateDto)
        {
            _logger.LogInformation("Démarrage de l'ajout de la pizza : {PizzaName}", pizzaCreateDto.Nom);

            // Créer une nouvelle pizza
            var pizza = new Pizza
            {
                Nom = pizzaCreateDto.Nom,
                Prix = pizzaCreateDto.Prix,
                PizzaIngredients = new List<PizzaIngredient>()
            };

            // Ajouter tous les ingrédients sélectionnés
            if (pizzaCreateDto.IngredientsIds != null && pizzaCreateDto.IngredientsIds.Count > 0)
            {
                foreach (var ingredientId in pizzaCreateDto.IngredientsIds)
                {
                    var ingredient = await _context.Ingredients.FindAsync(ingredientId);
                    if (ingredient == null)
                    {
                        _logger.LogError("L'ingrédient avec l'ID {IngredientId} n'existe pas.", ingredientId);
                        throw new ArgumentException($"L'ingrédient avec l'ID {ingredientId} n'existe pas.");
                    }

                    pizza.PizzaIngredients.Add(new PizzaIngredient
                    {
                        IngredientId = ingredientId
                    });
                }
            }
            else
            {
                _logger.LogWarning("Aucun ingrédient n'a été sélectionné pour la pizza : {PizzaName}", pizzaCreateDto.Nom);
                throw new ArgumentException("Aucun ingrédient n'a été sélectionné.");
            }

            // Enregistrer la pizza dans la base de données
            _context.Pizzas.Add(pizza);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Pizza ajoutée avec succès : {PizzaName}", pizzaCreateDto.Nom);
            return pizza;
        }

        public async Task<Pizza> UpdatePizzaAsync(int id, PizzaUpdateDto pizzaUpdateDto)
        {
            _logger.LogInformation("Démarrage de la mise à jour de la pizza avec l'ID {PizzaId}", id);

            var pizza = await _context.Pizzas.Include(p => p.PizzaIngredients).FirstOrDefaultAsync(p => p.Id == id);
            if (pizza == null)
            {
                _logger.LogWarning("Pizza avec l'ID {PizzaId} non trouvée.", id);
                return null;
            }

            pizza.Nom = pizzaUpdateDto.Nom;
            pizza.Prix = pizzaUpdateDto.Prix;
            pizza.PizzaIngredients = pizzaUpdateDto.IngredientsIds.Select(id => new PizzaIngredient { IngredientId = id }).ToList();

            _context.Pizzas.Update(pizza);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Pizza mise à jour avec succès : {PizzaName}", pizzaUpdateDto.Nom);
            return pizza;
        }

        public async Task<Pizza> GetPizzaByIdAsync(int id)
        {
            _logger.LogInformation("Récupération de la pizza avec l'ID {PizzaId}", id);

            var pizza = await _context.Pizzas.Include(p => p.PizzaIngredients).FirstOrDefaultAsync(p => p.Id == id);

            if (pizza == null)
            {
                _logger.LogWarning("Pizza avec l'ID {PizzaId} non trouvée.", id);
            }

            return pizza;
        }

        public async Task<PagedResult<PizzaDto>> GetAllPizzasAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation("Récupération de toutes les pizzas (page {PageNumber}, taille {PageSize})", pageNumber, pageSize);

            var query = _context.Pizzas
                .Include(p => p.PizzaIngredients)
                .ThenInclude(pi => pi.Ingredient)
                .AsQueryable();

            int totalRecords = await query.CountAsync();

            var pizzas = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(pizza => new PizzaDto
                {
                    Id = pizza.Id,
                    Nom = pizza.Nom,
                    Prix = pizza.Prix,
                    Ingredients = pizza.PizzaIngredients
                        .Select(pi => pi.Ingredient.Name)
                        .ToList()
                })
                .ToListAsync();

            _logger.LogInformation("Pizzas récupérées avec succès. Nombre total de pizzas : {TotalRecords}", totalRecords);

            return new PagedResult<PizzaDto>(pizzas, pageNumber, pageSize, totalRecords);
        }

        public async Task DeletePizza(int id)
        {
            _logger.LogInformation("Suppression de la pizza avec l'ID {PizzaId}", id);

            var pizza = await _context.Pizzas.FindAsync(id);
            if (pizza != null)
            {
                _context.Pizzas.Remove(pizza);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Pizza avec l'ID {PizzaId} supprimée avec succès", id);
            }
            else
            {
                _logger.LogWarning("Pizza avec l'ID {PizzaId} non trouvée pour suppression", id);
            }
        }

        public async Task<List<IngredientDto>> GetAllIngredientsAsync()
        {
            _logger.LogInformation("Récupération de tous les ingrédients");

            var ingredients = await _context.Ingredients
                .Select(i => new IngredientDto
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .ToListAsync();

            _logger.LogInformation("Ingrédients récupérés avec succès : {IngredientCount}", ingredients.Count);
            return ingredients;
        }
    }
}
