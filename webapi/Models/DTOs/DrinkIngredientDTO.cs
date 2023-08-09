using System.ComponentModel.DataAnnotations;
using webapi.Helpers;

namespace webapi.Models.DTOs
{
	public class DrinkIngredientDTO
	{
		public int IngredientId { get; set; }
		[Required]
		public string Name { get; set; }
		public string? Status { get; set; } // 1: ACTIVE 2:INACTIVE 3:DELETED 
		public int? Amount { get; set; }
		public UnitDTO Unit { get; set; }

	}
}