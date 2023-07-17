using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;
using System.Drawing;
using System.Net.Http.Headers;
using webapi.Data;
using webapi.Helpers;
using webapi.Models.DomainModels;
using webapi.Models.DTOs;
using static System.Net.Mime.MediaTypeNames;
using Image = SixLabors.ImageSharp.Image;
using Type = webapi.Models.DomainModels.Type;

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
		[DisableRequestSizeLimit]
		public async Task<IActionResult> AddDrink()
        {
			var formCollection = await Request.ReadFormAsync();

			var file = formCollection.Files.GetFile("file");

			// Access other form data values
			var addDrinkRequestJson = formCollection["addDrinkRequest"];

			// Deserialize the JSON to your model
			var addDrinkRequest = JsonConvert.DeserializeObject<UpdateDrinkRequest>(addDrinkRequestJson);

			if (file.Length > 0)
			{
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "TEMP_" + file.FileName);

				try
				{
					// Resize the image and save it temporarily in jpeg format.
					var image = Image.Load(file.OpenReadStream());
					image.Mutate(x => x.Resize(300, 300));
					await image.SaveAsJpegAsync(filePath);
					// Convert the file to byte[] and update db.
					var imageData = await System.IO.File.ReadAllBytesAsync(filePath);

					//Add a new Photo to db.
					var photo = new Photo
					{
						Image = imageData
					};
					await _dbContext.Photos.AddAsync(photo);
                    await _dbContext.SaveChangesAsync();

					// Delete the file
					System.IO.File.Delete(filePath);

					//Convert the DrinkDTO to Drink Domain Model
					var drink = new Drink
					{
						Name = addDrinkRequest.Name,
						Description = addDrinkRequest.Description,
						IsActive = true,
						DateCreated = DateTime.Now,
						Photo = photo,
						PhotoId = photo.Id
					};

					await _dbContext.Drinks.AddAsync(drink); // Drink object is being tracked by the context 
					await _dbContext.SaveChangesAsync();

					var drinkTypes = new List<DrinkType>();
					foreach (var type in addDrinkRequest.Types)
					{
						drinkTypes.Add(new DrinkType
						{
							DrinkId = drink.Id,
							TypeId = type.Id
						});
					}
					drink.DrinkTypes = drinkTypes;
					
					var drinkIngredients = new List<DrinkIngredient>();
					foreach (var ingredient in addDrinkRequest.DrinkIngredients)
					{
						drinkIngredients.Add(new DrinkIngredient
						{
							DrinkId = drink.Id,
							IngredientId = ingredient.IngredientId,
							Amount = ingredient.Amount ?? 0
						});
					}
					drink.DrinkIngredients = drinkIngredients;

					await _dbContext.SaveChangesAsync();

					return Ok();
				}
				catch (Exception ex)
				{
					if (System.IO.File.Exists(filePath))
					{
						// Delete the file
						System.IO.File.Delete(filePath);
					}
					return BadRequest(ex.Message);
				}
			}
			return NotFound();
		}

        // Get all drinks that are not deactivated
        // Make Asynch to allow the server to handle more requests concurrently (when involving I/O-bound operations to improve  scalability)
        [HttpGet]
        public async Task<IActionResult> GetAllDrinks()
        {
			// From dbSet to DTO
			var drinks = await _dbContext.Drinks.Join(
                            _dbContext.Photos,
                            drink => drink.PhotoId,
                            photo => photo.Id,
                            (drink, photo) => new DrinkDTO
                            {
                                Id = drink.Id,
                                Name = drink.Name,
                                Description = drink.Description,
                                IsActive = drink.IsActive,
                                DateCreated = drink.DateCreated,
                                Image = photo.Image,
					        }
			            ).ToListAsync();

			return Ok(drinks);
        }



        // Get drink by id
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetDrinkById([FromRoute] int id)
        {
            // Get domain object
			var types = await (from drink in _dbContext.Drinks
						 join drinkType in _dbContext.DrinkTypes
							on drink.Id equals drinkType.DrinkId
						 join type in _dbContext.Types
							on drinkType.TypeId equals type.Id
						 where drink.Id == id
						select (new TypeDTO
						{
							Id = type.Id,
							Name = type.Name
						})).ToListAsync();

			var drinkIngredients = await (from drink in _dbContext.Drinks
									join drinkIngredient in _dbContext.DrinkIngredients
										on drink.Id equals drinkIngredient.DrinkId
									join ingredient in _dbContext.Ingredients
										on drinkIngredient.IngredientId equals ingredient.Id
									join unit in _dbContext.Units
										on ingredient.UnitId equals unit.Id
									where drink.Id == id
									select (new DrinkIngredientDTO
									{
										IngredientId = ingredient.Id,
										Name = ingredient.Name,
										Amount = drinkIngredient.Amount,
										Unit = new UnitDTO { Id = unit.Id, Name = unit.Name },
										Status = ((StatusEnum)ingredient.StatusId).ToString()
									})).ToListAsync();

			var drinkFound = await (from drink in _dbContext.Drinks
							join photo in _dbContext.Photos
								on drink.PhotoId equals photo.Id
							where drink.Id == id
							select new DrinkDTO
							{
								Id = drink.Id,
								Name = drink.Name,
								Description = drink.Description,
								IsActive = drink.IsActive,
								DateCreated = drink.DateCreated,
								Image = photo.Image,
								Types = types,
								DrinkIngredients = drinkIngredients
							}).FirstOrDefaultAsync();

			if (drinkFound != null)
            {
                return Ok(drinkFound);
            }
            return BadRequest();
        }

        // Update drink with given data
        [HttpPut]
        [Route("Photo/{id:int}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateDrinkPhoto([FromRoute] int id)
        {
            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "TEMP_" + file.FileName);

                try
                {
                    // Resize the image and save it temporarily in jpeg format.
                    var image = Image.Load(file.OpenReadStream());
					image.Mutate(x => x.Resize(300, 300));
					await image.SaveAsJpegAsync(filePath);
                    // Convert the file to byte[] and update db.
					var imageData = await System.IO.File.ReadAllBytesAsync(filePath);

					// Get domain object
					var drink = await _dbContext.Drinks.FindAsync(id);
                    var photo = await _dbContext.Photos.FirstOrDefaultAsync(x => x.Id == drink.PhotoId);

					photo.Image = imageData;
					_dbContext.Update(photo);
					await _dbContext.SaveChangesAsync();

					// Delete the file
					System.IO.File.Delete(filePath);
					return Ok();
				}
                catch (Exception ex)
                {
					if (System.IO.File.Exists(filePath))
					{
						// Delete the file
						System.IO.File.Delete(filePath);
					}
					return BadRequest(ex.Message);
                }
			}
            

            return NotFound();
        }

		// Update drink with given data
		[HttpPut]
		[Route("{id:int}")]
		public async Task<IActionResult> UpdateDrink([FromRoute] int id, [FromBody] UpdateDrinkRequest updateDrinkRequest)
		{
			// Get domain object from DB
			var dbDrink = await _dbContext.Drinks.FindAsync(id);

			if (dbDrink != null)
			{
				dbDrink.Name = updateDrinkRequest.Name;
				dbDrink.Description = updateDrinkRequest.Description;
				dbDrink.IsActive = updateDrinkRequest.IsActive;
				dbDrink.DrinkIngredients = _dbContext.DrinkIngredients.Where(x => x.DrinkId == id).ToList();

				// Iterate through the drink ingredient list of DB
				foreach (var i in dbDrink.DrinkIngredients)
				{
					// If the updateDrinkRequest doesn't have the ingredient that is in DB (meaning the ingredient has been removed by user)
					if (!updateDrinkRequest.DrinkIngredients.Any(x => x.IngredientId == i.IngredientId))
					{
						// The ingredient needs to be removed from DB
						_dbContext.DrinkIngredients.Remove(i);
					}
					else
					{
						// The ingredient exists in DB. Update the Amount value.
						i.Amount = updateDrinkRequest.DrinkIngredients.First(x => x.IngredientId == i.IngredientId).Amount ?? 0;
					}
				}
				// Interate through the drink ingredient list provided by front-end
				foreach(var i in updateDrinkRequest.DrinkIngredients)
				{
					// If DB doesn't have the ingredient that is in updateDrinkRequest (meaning the ingredient is newly added)
					if (!dbDrink.DrinkIngredients.Any(x => x.IngredientId == i.IngredientId))
					{
						// The ingredient needs to be added to DB
						var newIngredient = new DrinkIngredient
						{
							DrinkId = id,
							Drink = dbDrink,
							IngredientId = i.IngredientId,
							Amount = i.Amount ?? 0
						};
						await _dbContext.DrinkIngredients.AddAsync(newIngredient);
					}
				}
				
				_dbContext.Update(dbDrink);
				await _dbContext.SaveChangesAsync();

				return Ok();
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
                _dbContext.Remove(drink.Photo);
				_dbContext.Remove(drink.DrinkTypes);
				_dbContext.Remove(drink.DrinkIngredients);
				_dbContext.Remove(drink);
                await _dbContext.SaveChangesAsync();

                return Ok(drink);
            }
            return NotFound();
        }

    }
}
