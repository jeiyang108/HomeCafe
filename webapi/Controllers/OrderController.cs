using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Helpers;
using webapi.Models.DomainModels;
using webapi.Models.DTOs;

namespace webapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : Controller
	{

		private readonly CafeDbContext _dbContext;

		public OrderController(CafeDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		[Route("{id:int}")]
		public async Task<IActionResult> ViewCart([FromRoute] int orderId)
		{
			var orderedDrinks = await (from order in _dbContext.Orders
									   join orderedDrink in _dbContext.OrderedDrinks
										on order.Id equals orderedDrink.Order.Id
									   join status in _dbContext.Statuses
										on orderedDrink.Status.Id equals status.Id
									   join drink in _dbContext.Drinks
										on orderedDrink.Drink.Id equals drink.Id
									   join photo in _dbContext.Photos
										on drink.Id equals photo.Id
									   where order.Id == orderId
									   select new OrderedDrinkDTO
									   {
										   Id = orderedDrink.Id,
										   OrderId = orderId,
										   DrinkId = drink.Id,
										   Name = drink.Name,
										   Image = photo.Image,
										   Sequence = orderedDrink.Sequence,
										   Status = new StatusDTO { Id = orderedDrink.Status.Id, Name = orderedDrink.Status.Name }
									   }).ToListAsync();

			var orderEntity = await (from order in _dbContext.Orders
									 join status in _dbContext.Statuses
										on order.Status.Id equals status.Id
									 join user in _dbContext.Users
										on order.User.Id equals user.Id
									 where order.Id == orderId
									 select new OrderDTO
									 {
										 Id = orderId,
										 UserId = user.Id,
										 UserName = user.Name,
										 DateCreated = order.DateCreated,
										 Status = new StatusDTO { Id = order.Status.Id, Name = order.Status.Name },
										 Notes = order.Notes,
										 OrderedDrinks = orderedDrinks
 									}).FirstOrDefaultAsync();

			if (orderEntity == null)
			{
				NotFound();
			}
			return Ok(orderEntity);
		}

		// Add a drink to the cart
		[HttpPost]
		public async Task<IActionResult> AddDrinkToCart([FromBody] OrderedDrinkDTO orderedDrink)
		{
			var orderDomain = await _dbContext.Orders.FindAsync(orderedDrink.OrderId);
			var numOfDrinks = await _dbContext.OrderedDrinks.CountAsync(d => d.Order.Id == orderedDrink.OrderId);
			var drinkDomain = await _dbContext.Drinks.FindAsync(orderedDrink.DrinkId);

			if (orderDomain == null || drinkDomain == null)
			{
				return NotFound();
			}

			// Add a new drink to the order
			var newDrink = new OrderedDrink
			{
				Order = orderDomain,
				Drink = drinkDomain,
				Status = new Status { Id = (int)StatusEnum.NewOrder, Name = "New Order" },
				Sequence = numOfDrinks + 1
			};
			await _dbContext.OrderedDrinks.AddAsync(newDrink); // OrderedDrink object is being tracked by the context 
			await _dbContext.SaveChangesAsync();

			// Add ingredients to the drink
			foreach (var ingredient in orderedDrink.OrderedDrinkIngredients)
			{
				var newIngredient = new OrderedDrinkIngredient
				{
					OrderedDrinkId = newDrink.Id,
					IngredientId = ingredient.IngredientId,
					Amount = ingredient.Amount
				};
				await _dbContext.OrderedDrinkIngredients.AddAsync(newIngredient);
			}
			await _dbContext.SaveChangesAsync();
			return Ok();
		}
	}
}
