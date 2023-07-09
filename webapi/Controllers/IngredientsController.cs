using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using webapi.Data;
using webapi.Helpers;
using webapi.Models.DTOs;

namespace webapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IngredientsController : Controller
	{
		private readonly CafeDbContext _dbContext;

		public IngredientsController(CafeDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllIngredients()
		{
			var ingredients = await _dbContext.Ingredients.Where(i => i.Status != StatusEnum.Deleted).Join(
								_dbContext.Units,
								ingredient => ingredient.UnitId,
								unit => unit.Id,
								(ingredient, unit) => new DrinkIngredientDTO
								{
									IngredientId = ingredient.Id,
									IngredientName = ingredient.Name,
									UnitName = unit.Name,
									IngredientStatus = ingredient.Status.ToString(),
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
			ingredient.Status = statusId;
			return Ok(ingredient);
		}
	}
}
