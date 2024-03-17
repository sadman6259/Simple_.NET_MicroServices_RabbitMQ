namespace NotificationService
{
    public class EmailInfo
    {
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public Guid RequestId { get; set; }
    }
}