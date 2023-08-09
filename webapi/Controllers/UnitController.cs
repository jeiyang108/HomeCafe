using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using webapi.Data;
using webapi.Helpers;
using webapi.Models.DomainModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UnitController : ControllerBase
	{
		private readonly CafeDbContext _dbContext;
		public UnitController(CafeDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllUnits()
		{
			var units = await _dbContext.Units.ToListAsync();
			return Ok(units);
		}
	}
}
