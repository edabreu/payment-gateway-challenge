using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Dtos;
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
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(Payment), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ProcessPaymentAsync(
        [FromHeader(Name = "X-merchant_id")] string merchantId,
        [FromBody]PaymentRequest paymentRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Will process payment request with reference {reference}", new { paymentRequest.Reference });

        var paymentResponse = await _paymentService.ProcessAsync(merchantId, paymentRequest);

        return CreatedAtAction(
            "GetPaymentDetails",
            new { id = paymentResponse.Id },
            paymentResponse);
    }

    [HttpGet("{id}", Name = "GetPaymentDetails")]
    [ActionName("GetPaymentDetails")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(Payment), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPaymentDetailsAsync(
        [FromHeader(Name = "X-merchant_id")] string merchantId,
        [FromRoute] string id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var payment = await _paymentService.GetAsync(id);

        if (payment is null)
        {
            return NotFound();
        }
        else if (!string.Equals(payment.Merchant, merchantId))
        {
            return Unauthorized();
        }

        return Ok(payment);
    }
}