using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [Route("ProcessPayment")]
        [HttpPost]
        public IActionResult ProcessPayment(PaymentInfo paymentInfo)
        {
            try
            {
                _logger.LogInformation("payment processing completed from paymentService: " + paymentInfo.RequestId);
                return Ok(paymentInfo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}