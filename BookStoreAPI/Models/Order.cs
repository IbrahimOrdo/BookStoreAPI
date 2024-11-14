namespace BookStoreAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public Payment Payment { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }

}
