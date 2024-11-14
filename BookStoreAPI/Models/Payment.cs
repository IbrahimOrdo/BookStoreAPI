namespace BookStoreAPI.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string? CardNumber { get; set; }
        public string? CardHolderName { get; set; }
        public string ExpirationDate { get; set; }
        public string? CVV { get; set; }
        public decimal Amount { get; set; }
        public int OrderId { get; set; }
        public Order order { get; set; }
    }

}
