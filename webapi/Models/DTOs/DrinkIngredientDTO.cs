using System.ComponentModel.DataAnnotations;

namespace webapi.Models.DTOs
{
	public class DrinkIngredientDTO
	{
		public int IngredientId { get; set; }
		[Required]
		public string IngredientName { get; set; }
		public int Amount { get; set; }
		public string UnitName { get; set; }

	}
}