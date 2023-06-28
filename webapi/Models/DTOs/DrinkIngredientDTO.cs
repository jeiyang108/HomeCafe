namespace webapi.Models.DTOs
{
	public class DrinkIngredientDTO
	{
		public DrinkDTO Drink { get; set; }
		public IngredientDTO Ingredient { get; set; }
		public int Amount { get; set; }
	}
}