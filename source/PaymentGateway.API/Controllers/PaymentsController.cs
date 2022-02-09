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
    [ProducesResponseType(typeof(PaymentResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaymentResponse>> ProcessPaymentAsync(
        [FromHeader(Name = "X-merchant_id")] string merchantId,
        [FromBody]PaymentRequest paymentRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Will process payment request with reference {reference}", new { paymentRequest.Reference });

        var paymentResponse = await _paymentService.ProcessAsync(paymentRequest);

        return paymentResponse;
        /*return CreatedAtAction(
            nameof(GetPaymentDetailsAsync),
            new { merchantId = merchantId, id = paymentResponse.Id },
            paymentResponse);/**/
    }

    /*[HttpGet("{reference}", Name = "GetPaymentDetails")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(PaymentResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaymentResponse>> GetPaymentDetailsAsync(
        [FromHeader(Name = "X-merchant_id")] string merchantId,
        [FromRoute] string id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return new PaymentResponse();
    }/**/
}