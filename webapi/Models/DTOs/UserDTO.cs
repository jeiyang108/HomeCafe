using System.ComponentModel.DataAnnotations;

namespace webapi.Models.DTOs
{
	public class UserDTO
	{
		[Required]
		public int Id { get; set; }
		[Required]
		[StringLength(50)]
		public string Name { get; set; }
		public string Password { get; set; }
		[Required]
		public string Email { get; set; }
		public DateTime DateCreated { get; set; }
		public bool IsAdmin { get; set; }
		public bool EmailConfirmed { get; set; }
		public ICollection<OrderDTO> Orders { get; set; }
	}
}
