using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pizza_app.DTO.PizzaDTO;
using pizza_app.Entities.Pizzas;
using pizza_app.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace pizza_app.Controllers
{
    [ApiController]
    [Route("admin/pizzas")]
  
    public class PizzasController : ControllerBase
    {
        private readonly IPizzaService _pizzaService;

        public PizzasController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }


        // GET /admin/pizzas
        [HttpGet]
        [Authorize]

        [SwaggerOperation(Summary = "Récupère toutes les pizzas avec pagination", Description = "Retourne une liste paginée de pizzas.")]

        public async Task<IActionResult> GetAllPizzas(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var pagedResult = await _pizzaService.GetAllPizzasAsync(pageNumber, pageSize);
                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur interne est survenue." });
            }
        }

        [SwaggerOperation(Summary = "Récupère une pizza par ID", Description = "Retourne les détails d'une pizza spécifique.")]

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetPizzaById(int id)
        {
            try
            {
                var pizza = await _pizzaService.GetPizzaByIdAsync(id);
                if (pizza == null)
                {
                    return NotFound(new { message = "Pizza non trouvée." });
                }
                return Ok(pizza);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur interne est survenue." });
            }
        }

        // POST /admin/pizzas
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Crée une nouvelle pizza", Description = "Crée une nouvelle pizza dans le système.")]
        [SwaggerResponse(201, "Pizza créée avec succès", typeof(PizzaDto))]
        [SwaggerResponse(400, "Demande invalide")]
        public async Task<IActionResult> CreatePizza([FromBody] PizzaCreateDto pizzaCreateDto)
        {
            try
            {
                var pizza = await _pizzaService.AddPizzaAsync(pizzaCreateDto);
                //_logger.LogInformation("Pizza avec l'ID {PizzaId} créée avec succès.", pizza.Id);
                return CreatedAtAction(nameof(GetPizzaById), new { id = pizza.Id }, new { id = pizza.Id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur interne est survenue." });
            }
        }

        // PUT /admin/pizzas/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Met à jour une pizza", Description = "Met à jour les informations d'une pizza existante.")]
        [SwaggerResponse(200, "Pizza mise à jour avec succès", typeof(PizzaDto))]
        [SwaggerResponse(400, "Demande invalide")]
        [SwaggerResponse(404, "Pizza non trouvée")]
        public async Task<IActionResult> UpdatePizza(int id, [FromBody] PizzaUpdateDto pizzaUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedPizza = await _pizzaService.UpdatePizzaAsync(id, pizzaUpdateDto);
            if (updatedPizza == null)
            {
                return NotFound(new { message = "Pizza non trouvée." });
            }

            return Ok(updatedPizza);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Supprime une pizza", Description = "Supprime une pizza spécifique par ID.")]
        [SwaggerResponse(204, "Pizza supprimée avec succès")]
        [SwaggerResponse(404, "Pizza non trouvée")]
        public async Task<IActionResult> DeletePizza(int id)
        {
            try
            {
                var pizza = await _pizzaService.GetPizzaByIdAsync(id);
                if (pizza == null)
                {
                    return NotFound(new { message = "Pizza non trouvée." });
                }

                _pizzaService.DeletePizza(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur interne est survenue." });
            }
        }

        // GET /admin/ingredients
        [HttpGet("ingredients")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Récupère tous les ingrédients", Description = "Retourne la liste de tous les ingrédients disponibles.")]
        public async Task<IActionResult> GetAllIngredients()
        {
            try
            {
                var ingredients = await _pizzaService.GetAllIngredientsAsync();
                return Ok(ingredients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur interne est survenue." });
            }
        }

    }
}
