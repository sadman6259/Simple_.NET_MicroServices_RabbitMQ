using Microsoft.AspNetCore.Mvc;
using NotificationService.rabbitMQ;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        

        private readonly ILogger<EmailController> _logger;
        public IRabbitMQUtil _rabbitMQUtil;

        public EmailController(ILogger<EmailController> logger, IRabbitMQUtil rabbitMQUtil)
        {
            _logger = logger;
            _rabbitMQUtil = rabbitMQUtil;
        }

        [HttpPost(Name = "SendEmail")]
        public IActionResult SendEmail(EmailInfo emailInfo)
        {
            try
            {
                _logger.LogInformation("Send Email start from NotificationService: " );

                return Ok(emailInfo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}