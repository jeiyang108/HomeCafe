using System.ComponentModel.DataAnnotations;
using webapi.Helpers;

namespace webapi.Models.DTOs
{
	public class DrinkIngredientDTO
	{
		public int IngredientId { get; set; }
		[Required]
		public string IngredientName { get; set; }
		public string? IngredientStatus { get; set; } // 1: ACTIVE 2:INACTIVE 3:DELETED 
		public int? Amount { get; set; }
		public string? UnitName { get; set; }

	}
}