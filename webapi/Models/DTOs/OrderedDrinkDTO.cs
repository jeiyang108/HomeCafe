namespace webapi.Models.DTOs
{
	public class OrderedDrinkDTO
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int DrinkId { get; set; }
		public string Name { get; set; }
		public byte[] Image { get; set; }
		//public DrinkDTO Drink { get; set; }
		public StatusDTO Status { get; set; }
		public int Sequence { get; set; }
		public ICollection<OrderedDrinkIngredientDTO> OrderedDrinkIngredients { get; set;}

	}
}