namespace OrderApp_Microservices
{
    public class Order
    {
        public DateTime CreatedDate { get; set; }

        public Guid RequestId { get; set; }

        public string MenuName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string? CustomerName{ get; set; }
        public string? CustomerEmail { get; set; }
        public string? AccountNo { get; set; }


    }
}