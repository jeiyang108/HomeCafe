using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using webapi.Data;
using webapi.Helpers;
using webapi.Models.DomainModels;
using webapi.Models.DTOs;

namespace webapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IngredientController : Controller
	{
		private readonly CafeDbContext _dbContext;

		public IngredientController(CafeDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllIngredients()
		{
			var ingredients = await _dbContext.Ingredients.Join(
								_dbContext.Units, 
								ingredient => ingredient.UnitId, 
								unit => unit.Id, 
								(ingredient, unit)  => new DrinkIngredientDTO
								{
									IngredientId = ingredient.Id,
									Name = ingredient.Name,
									Unit = new UnitDTO { Id = unit.Id, Name = unit.Name },
									Status = ((StatusEnum?)ingredient.StatusId).ToString(),
								}).ToListAsync();

			if (ingredients.Any())
			{
				return Ok(ingredients);
			}
			return NotFound();
		}

		[HttpPut]
		[Route("{id:int}")]
		public async Task<IActionResult> UpdateIngredientStatus(int id, string action)
		{
			var ingredient = await _dbContext.Ingredients.FindAsync(id);
			if (ingredient == null)
			{
				return NotFound();
			}
			var statusId = Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().FirstOrDefault(x => x.ToString() == action);
			ingredient.StatusId = (int)statusId;

			_dbContext.Ingredients.Update(ingredient);
			await _dbContext.SaveChangesAsync();

			return Ok(ingredient);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateIngredient([FromBody] DrinkIngredientDTO model)
		{
			var ingredient = await _dbContext.Ingredients.FindAsync(model.IngredientId);
			
			if (ingredient == null || model.Status == null)
			{
				return NotFound();
			}
			ingredient.Name = model.Name;
			ingredient.UnitId = model.Unit.Id;

			var newStatus = (StatusEnum) Enum.Parse(typeof(StatusEnum), model.Status);
			
			ingredient.StatusId = (int) newStatus;
			_dbContext.Ingredients.Update(ingredient);
			
			// Activate or deactivate the drinks based on the ingredient's availability(Status)
			UpdateDrinkStatus(ingredient.Id, StatusEnum.Active == newStatus);

			await _dbContext.SaveChangesAsync();
			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> AddIngredient([FromBody] DrinkIngredientDTO model)
		{
			var ingredient = new Ingredient
			{
				Name = model.Name,
				StatusId = (int)Enum.Parse(typeof(StatusEnum), model.Status ?? "Active"),
				UnitId = model.Unit.Id,
			};

			await _dbContext.Ingredients.AddAsync(ingredient);
			await _dbContext.SaveChangesAsync();
			return Ok(ingredient);
		}

		private async Task UpdateDrinkStatus(int ingredientId, bool activate)
		{
			// Find all drinks that are made with the ingredient
			var drinks = await (from drink in _dbContext.Drinks
								join drinkIngredient in _dbContext.DrinkIngredients
									on drink.Id equals drinkIngredient.DrinkId
								where drinkIngredient.IngredientId == ingredientId
								select drink).ToListAsync();
			
			if (activate)
			{
				foreach (var drink in drinks)
				{
					// Needs to check if the dirnk has any other deactivated ingredient
					var otherIngredients = await (from drinkIngredient in _dbContext.DrinkIngredients
												  join ingredient in _dbContext.Ingredients
													on drinkIngredient.IngredientId equals ingredient.Id
												  where drinkIngredient.DrinkId == drink.Id && ingredient.StatusId == (int)StatusEnum.Inactive
												  select ingredient).ToListAsync();
					if (otherIngredients.Count == 1)
					{
						// Set IsActive to true (Activate)
						drinks.ForEach(e => e.IsActive = activate);
					}
					// If the drink has more than 1 deactivated ingredients, do not re-activate the drink.
				}
			}
			else
			{
				// Set IsActive to false (Deactivate)
				drinks.ForEach(e => e.IsActive = activate);
			}
			
			_dbContext.Drinks.UpdateRange(drinks);
		}
	}
}
