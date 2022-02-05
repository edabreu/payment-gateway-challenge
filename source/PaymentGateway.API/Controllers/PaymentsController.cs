using System.Net;
using Microsoft.AspNetCore.Mvc;
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
        return await _paymentService.ProcessAsync(paymentRequest);
    }
}

public class PaymentResponse
{
    public PaymentResponse()
    {
    }
}

public class PaymentRequest
{
    public string Reference { get; set; }
    public int Amount { get; set; }
    public string Currency { get; set; }
    public CardDetails CardDetails { get; set; }
}

public class CardDetails
{
    public string PrimaryAccountNumber { get; set; }
    public ExpirationDate expirationDate { get; set; }
    public string CVV { get; set; }
}

public class ExpirationDate
{
    public int Month { get; set; }
    public int Year { get; set; }
}