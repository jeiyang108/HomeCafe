using System.ComponentModel.DataAnnotations;
using webapi.Models.DomainModels;

namespace webapi.Models.DTOs
{
	public class IngredientDTO
	{
		[Required]
		public int Id { get; set; }
		public string Name { get; set; }
		public Unit Unit { get; set; }

		public int UnitId { get; set; }
	}
}
