using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Net.Http.Headers;
using webapi.Data;
using webapi.Models.DomainModels;
using webapi.Models.DTOs;
using static System.Net.Mime.MediaTypeNames;
using Image = SixLabors.ImageSharp.Image;

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
		public async Task<IActionResult> AddDrink([FromBody] AddDrinkRequest addDrinkRequest)
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

					await _dbContext.Drinks.AddAsync(drink);
					await _dbContext.SaveChangesAsync();

					return Ok(drink);
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
                                Image = photo.Image
					        }
			            ).ToListAsync();

			//System.IO.File.WriteAllBytes("Images/Test.jpg", drinks.FirstOrDefault().Image);

			return Ok(drinks);
        }

        // Get drink by id
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetDrinkById([FromRoute] int id)
        {
            // Get domain object
            var drink = await _dbContext.Drinks.Join(
                            _dbContext.Photos,
                            drink => drink.Id,
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
			            ).FirstOrDefaultAsync(x => x.Id == id);

			if (drink != null)
            {
                return Ok(drink);
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
