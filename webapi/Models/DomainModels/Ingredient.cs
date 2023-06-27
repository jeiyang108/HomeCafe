namespace webapi.Models.DomainModels
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Unit Unit { get; set; }

        public int UnitId { get; set; }
    }
}
