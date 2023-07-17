using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using webapi.Data;
using webapi.Helpers;
using webapi.Models.DomainModels;
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
			var ingredients = await _dbContext.Ingredients.Where(i => i.StatusId != (int)StatusEnum.Deleted).
								Select(ingredient => new DrinkIngredientDTO
								{
									IngredientId = ingredient.Id,
									Name = ingredient.Name,
									Unit = new UnitDTO { Id = ingredient.UnitId, Name = ((UnitEnum?)ingredient.UnitId).GetDisplayName() },
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
			if (ingredient == null)
			{
				return NotFound();
			}
			ingredient.Name = model.Name;
			ingredient.UnitId = (int)Enum.GetValues(typeof(UnitEnum)).Cast<UnitEnum>().FirstOrDefault(e => e.GetDisplayName() == model.Unit.Name);
			ingredient.StatusId = (int)Enum.Parse(typeof(StatusEnum), model.Status ?? "Active");

			_dbContext.Ingredients.Update(ingredient);
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
				UnitId = (int)Enum.GetValues(typeof(UnitEnum)).Cast<UnitEnum>().FirstOrDefault(e => e.GetDisplayName() == model.Unit.Name),
			};

			await _dbContext.Ingredients.AddAsync(ingredient);
			await _dbContext.SaveChangesAsync();
			return Ok(ingredient);
		}
	}
}
