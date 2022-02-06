using System.Net;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Models;
using PaymentGateway.API.Services;

namespace PaymentGateway.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpPost(Name = "ProcessPayment")]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(PaymentResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaymentResponse>> ProcessPaymentAsync([FromBody]PaymentRequest paymentRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Will process payment request with reference {reference}", new { paymentRequest.Reference });

        return await _paymentService.ProcessAsync(paymentRequest);
    }
}