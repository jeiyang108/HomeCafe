namespace webapi.Models.DTOs
{
	public class OrderDTO
	{
		public int Id { get; set; }
		public UserDTO User { get; set; }
		public DateTime DateCreated { get; set; }
		public StatusDTO Status { get; set; }
		public string Notes { get; set; }
		public ICollection<OrderedDrinkDTO> OrderedDrinks { get; set; }
	}
}