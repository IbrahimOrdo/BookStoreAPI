namespace BookStoreAPI.Models.Requests
{
    public class ProcessPaymentsRequest
    {
        public int orderId { get; set; }
        public string cardNumber { get; set; }
        public string cardHolderName { get; set; }
        public string expirationDate { get; set; }
        public string cvv { get; set; }

    }
}
