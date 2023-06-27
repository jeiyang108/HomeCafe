namespace webapi.Models.DomainModels
{
    public class OrderedDrink
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public Drink Drink { get; set; }
        public Status Status { get; set; }
        public int Sequence { get; set; }
    }
}
