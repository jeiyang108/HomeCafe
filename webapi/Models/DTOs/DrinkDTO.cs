namespace webapi.Models.DTOs
{
    public class DrinkDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public byte[]? Image { get; set; }
    }

    public class AddDrinkRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public byte[]? Image { get; set; }
    }

    public class UpdateDrinkRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public byte[]? Image { get; set; }
    }
}
