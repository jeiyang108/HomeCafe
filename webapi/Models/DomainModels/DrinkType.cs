namespace webapi.Models.DomainModels
{
    public class DrinkType
    {
        public Drink Drink { get; set; }
        public Type Type { get; set; }
        public int DrinkId { get; set; }
        public int TypeId { get; set; }


    }
}
