using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderService.DTO;
using OrderService.rabbitMQ;
using System.Net;
using System.Text.Json;

namespace OrderApp_Microservices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        
        private readonly ILogger<OrderController> _logger;
        private IRabbitMQUtil _rabbitMQUtil;

        public OrderController(ILogger<OrderController> logger, IRabbitMQUtil rabbitMQUtil)
        {
            _logger = logger;
            _rabbitMQUtil = rabbitMQUtil;
        }

        private PaymentInfoDTO MapPaymentDTO(Order order)
        {
            PaymentInfoDTO paymentInfoDTO = new PaymentInfoDTO();
            paymentInfoDTO.RequestId = order.RequestId;
            paymentInfoDTO.Price = order.Price;
            paymentInfoDTO.CustomerName = order.CustomerName;
            paymentInfoDTO.AccountNo = order.AccountNo;
            return paymentInfoDTO;

        }

        private EmailInfoDTO MapEmailDTO(Order order)
        {
            EmailInfoDTO emailInfoDTO = new EmailInfoDTO();
            emailInfoDTO.RequestId = order.RequestId;
            emailInfoDTO.CustomerName = order.CustomerName;
            emailInfoDTO.CustomerEmail = order.CustomerEmail;
            return emailInfoDTO;

        }

        [Route("PlaceOrder")]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(Order order)
        {
            try
            {
                _logger.LogInformation("Order received from orderService: " + order.RequestId);

                PaymentInfoDTO paymentInfoDTO = MapPaymentDTO(order);

                var PaymentServiceURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("PaymentServiceKeys")["PaymentServiceURL"];
                var ProcessPaymentEndPoint = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("PaymentServiceKeys")["PaymentProcessingEndPoint"];


                #region  call paymentservice

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(PaymentServiceURL);

                    var result = await client.PostAsJsonAsync(ProcessPaymentEndPoint, paymentInfoDTO);

                    if(result.StatusCode == HttpStatusCode.OK)
                    {
                        _logger.LogInformation("Payment Done Successfully for RequestId: " + order.RequestId);

                        #region publish message for Email
                        EmailInfoDTO emailInfoDTO = MapEmailDTO(order);

                        var emailData = JsonSerializer.Serialize(emailInfoDTO);

                        await _rabbitMQUtil.PublishMessage("order.notification", emailData);

                        _logger.LogInformation("Notification message Sent Successfully to MQ for RequestId: " + order.RequestId);

                        #endregion

                        return Ok(order);
                    }

                    _logger.LogInformation("ErrorCode received from PaymentService: " + result.StatusCode);

                }

                #endregion

               

                return BadRequest(order);


            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error from PlaceOrder: " + ex);
                throw;
            }
            
        }
    }
}