using webapi.Helpers;

namespace webapi.Models.DomainModels
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
		public int StatusId { get; set; }
		public Unit Unit { get; set; }

        public int UnitId { get; set; }
    }
}
