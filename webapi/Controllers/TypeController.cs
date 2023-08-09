using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;

namespace webapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TypeController : Controller
	{
		private readonly CafeDbContext _dbContext;

		public TypeController(CafeDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllTypes()
		{
			var types = await _dbContext.Types.ToListAsync();
			if (types.Any())
			{
				return Ok(types);
			}
			return NotFound();
		}
	}
}
