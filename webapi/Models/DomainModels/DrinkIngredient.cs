namespace webapi.Models.DomainModels
{
    public class DrinkIngredient
    {
        public Drink Drink { get; set; }
        public Ingredient Ingredient { get; set; }

        public int DrinkId { get; set; }
        public int IngredientId { get; set; }
        public int Amount { get; set; }
    }
}
