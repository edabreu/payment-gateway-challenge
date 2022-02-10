using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace CKOBank.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ILogger<PaymentsController> _logger;
    private IReadOnlyDictionary<string, CardProcessingResultInformation> _availableCards;

    public PaymentsController(ILogger<PaymentsController> logger)
    {
        _availableCards = new Dictionary<string, CardProcessingResultInformation>
        {
            { "1234567890123452", new("1234567890123452", true, "Authorized") },
            { "4643335951477921", new("4643335951477921", false, "InsuficientFunds") },
        };
        _logger = logger;
    }

    [HttpPost(Name = "ProcessPayment")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public ActionResult<PaymentResponse> ProcessPayment([FromBody]PaymentRequest paymentRequest)
    {
        _logger.LogInformation("Received payment request {reference}", new { paymentRequest.Reference });

        var response = new PaymentResponse { PaymentId = Guid.NewGuid().ToString() };
        if (_availableCards.TryGetValue(paymentRequest.CardNumber, out var responseInfo))
        {
            response.Approved = responseInfo.Approved;
            response.Status = responseInfo.Status;
        }
        else
        {
            response.Approved = false;
            response.Status = "InvalidCard";
        }

        return response;
    }
}

public record CardProcessingResultInformation(string Number, bool Approved, string Status);

public class PaymentResponse
{
    public string PaymentId { get; set; } = string.Empty;
    public bool Approved { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class PaymentRequest
{
    public string Reference { get; set; } = string.Empty;
    public int Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string CardNumber { get; set; } = String.Empty;
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public string CVV { get; set; } = String.Empty;
    public string HolderName { get; set; } = String.Empty;
}