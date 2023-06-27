using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.Xml;
using webapi.Data;
using webapi.Models.DomainModels;
using webapi.Models.DTOs;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrinksController : ControllerBase
    {

        private readonly CafeDbContext _dbContext;

        public DrinksController(CafeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        // Create a new drink
        [HttpPost]
        public async Task<IActionResult> AddDrink([FromBody] AddDrinkRequest addDrinkRequest)
        {
            //Convert the DTO to Domain Model
            var drink = new Drink
            {
                Name = addDrinkRequest.Name,
                Description = addDrinkRequest.Description,
                IsActive = true,
                DateCreated = DateTime.Now
            };

            await _dbContext.Drinks.AddAsync(drink);
            await _dbContext.SaveChangesAsync();

            return Ok(drink);
        }

        // Get all drinks that are not deactivated
        // Make Asynch to allow the server to handle more requests concurrently (when involving I/O-bound operations to improve  scalability)
        [HttpGet]
        public async Task<IActionResult> GetAllDrinks()
        {
            // From dbSet to DTO
            var drinks = await _dbContext.Drinks.Join(_dbContext.Photos,
                            drink => drink.PhotoId,
                            photo => photo.Id,
                            (drink, photo) => new DrinkDTO
                            {
                                Id = drink.Id,
                                Name = drink.Name,
                                Description = drink.Description,
                                IsActive = drink.IsActive,
                                DateCreated = drink.DateCreated,
                                Image = photo.Image
                            }
                         ).ToListAsync();
            /*
            var drinksDTO = new List<DrinkDTO>();
            foreach (var drink in drinks)
            {
                drinksDTO.Add(new DrinkDTO
                {
                    Id = drink.Id,
                    Name = drink.Name,
                    Description = drink.Description,
                    IsActive = drink.IsActive,
                    DateCreated = drink.DateCreated
                    Image = drink
                });
            }
            */

            return Ok(drinks);
        }

        // Get drink by id
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetDrinkById([FromRoute] int id)
        {
            // Get domain object
            var drink = await _dbContext.Drinks.FindAsync(id);

            if (drink != null)
            {
                var drinkDTO = new DrinkDTO
                {
                    Id = drink.Id,
                    Name = drink.Name,
                    Description = drink.Description,
                    IsActive = drink.IsActive,
                    DateCreated = drink.DateCreated,
                };

                return Ok(drinkDTO);
            }
            return BadRequest();
        }

        // Update drink with given data
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateDrink([FromRoute] int id, [FromBody] UpdateDrinkRequest updateDrinkRequest)
        {
            // Get domain object
            var drink = await _dbContext.Drinks.FindAsync(id);

            if (drink != null)
            {
                drink.Name = updateDrinkRequest.Name;
                drink.Description = updateDrinkRequest.Description;
                drink.IsActive = updateDrinkRequest.IsActive;

                _dbContext.Update(drink);
                await _dbContext.SaveChangesAsync();

                return Ok(drink);
            }
            return NotFound();
        }

        // Delete drink
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteDrink([FromRoute] int id)
        {
            // Get domain object
            var drink = await _dbContext.Drinks.FindAsync(id);

            if (drink != null)
            {
                _dbContext.Remove(drink);
                await _dbContext.SaveChangesAsync();

                return Ok(drink);
            }
            return NotFound();
        }
    }
}
