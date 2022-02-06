using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace CKOBank.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(ILogger<PaymentsController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "ProcessPayment")]
    [Consumes(MediaTypeNames.Application.Json)]
    public ActionResult<PaymentResponse> ProcessPayment([FromBody]PaymentRequest paymentRequest)
    {
        return new PaymentResponse();
    }
}

public class PaymentResponse
{
}

public class PaymentRequest
{
}