using System.ComponentModel.DataAnnotations;

namespace webapi.Models.DTOs
{
	public class OrderedDrinkIngredientDTO
	{
		[Required]
		public int OrderedDrinkId { get; set; }
		[Required]
		public int IngredientId { get; set; }
		public int Amount { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public string Unit { get; set; }
		
	}
}