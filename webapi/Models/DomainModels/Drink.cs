namespace webapi.Models.DomainModels
{
    public class Drink
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public int PhotoId { get; set; }
        public Photo Photo { get; set; }
        public IEnumerable<DrinkType> DrinkTypes { get; set; }
        public IEnumerable<DrinkIngredient> DrinkIngredients { get; set; }
    }
}
