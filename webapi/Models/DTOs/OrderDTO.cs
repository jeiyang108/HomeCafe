namespace webapi.Models.DTOs
{
	public class OrderDTO
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public DateTime DateCreated { get; set; }
		public StatusDTO Status { get; set; }
		public string Notes { get; set; }
		public ICollection<OrderedDrinkDTO> OrderedDrinks { get; set; }
	}
}