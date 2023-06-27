namespace webapi.Models.DomainModels
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime DateCreated { get; set; }
        public Status Status { get; set; }
        public string Notes { get; set; }
    }
}
