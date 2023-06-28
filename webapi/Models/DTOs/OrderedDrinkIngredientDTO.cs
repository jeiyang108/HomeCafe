namespace webapi.Models.DTOs
{
	public class OrderedDrinkIngredientDTO
	{
		public OrderedDrinkDTO OrderedDrink { get; set; }
		public IngredientDTO Ingredient { get; set; }
		public int Amount { get; set; }
	}
}