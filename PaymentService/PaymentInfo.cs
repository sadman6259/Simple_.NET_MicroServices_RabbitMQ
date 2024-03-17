namespace PaymentService
{
    public class PaymentInfo
    {
        public decimal Price { get; set; }

        public string AccountNo { get; set; }

        public string CustomerName { get; set; }

        public Guid RequestId { get; set; }

    }
}