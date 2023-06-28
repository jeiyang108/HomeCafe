using System.ComponentModel.DataAnnotations;

namespace webapi.Models.DTOs
{
    public class DrinkDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public byte[]? Image { get; set; }
        public ICollection<TypeDTO> Types { get; set; }
        public ICollection<DrinkIngredientDTO> DrinkIngredients { get; set;}
    }

    public class AddDrinkRequest
    {
		[Required]
		public string Name { get; set; }
        public string Description { get; set; }
		public ICollection<TypeDTO> Types { get; set; }
		public ICollection<DrinkIngredientDTO> DrinkIngredients { get; set; }
	}

    public class UpdateDrinkRequest
    {
		[Required]
		public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
		public ICollection<TypeDTO> Types { get; set; }
		public ICollection<DrinkIngredientDTO> DrinkIngredients { get; set; }
	}
}
