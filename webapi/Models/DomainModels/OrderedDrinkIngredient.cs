using Microsoft.Identity.Client;

namespace webapi.Models.DomainModels
{
    public class OrderedDrinkIngredient
    {
        public OrderedDrink OrderedDrink { get; set; }
        public Ingredient Ingredient { get; set; }
        public int Amount { get; set; }
        public int OrderedDrinkId { get; set; }
        public int IngredientId { get; set; }
    }
}
