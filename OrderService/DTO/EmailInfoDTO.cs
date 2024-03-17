namespace OrderService.DTO
{
    public class EmailInfoDTO
    {
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public Guid RequestId { get; set; }
    }
}
