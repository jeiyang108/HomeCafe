using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using webapi.Helpers;
using webapi.Models.DomainModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UnitsController : ControllerBase
	{
		public UnitsController()
		{
		}

		[HttpGet]
		public async Task<IActionResult> GetAllUnits()
		{
			var units = new List<Unit>();
			foreach (var item in Enum.GetValues<UnitEnum>())
			{
				units.Add(
					new Unit { Id = (int)item, Name = item.GetDisplayName() }
				);
			}
			return Ok(units);
		}
	}
}
