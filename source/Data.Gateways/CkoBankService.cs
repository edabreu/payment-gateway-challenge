using System.Net.Mime;
using System.Text.Json;
using Domain.Abstractions.Gateways;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Data.Gateways;

public class CkoBankService : ICkoBankService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CkoBankService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;

    public CkoBankService(HttpClient httpClient, ILogger<CkoBankService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    public async Task<Payment> ProcessPaymentAsync(PaymentRequest paymentRequest)
    {
        var ctoBankRequest = new CkoPaymentRequest
        {
            Amount = paymentRequest.Amount,
            CardNumber = paymentRequest.CardDetails.Number,
            Currency = paymentRequest.Currency,
            CVV = paymentRequest.CardDetails.CVV,
            ExpirationMonth = paymentRequest.CardDetails.ExpirationDate.Month,
            ExpirationYear = paymentRequest.CardDetails.ExpirationDate.Year,
            HolderName = paymentRequest.CardDetails.HolderName,
            Reference = paymentRequest.Reference
        };
        var content = new StringContent(
            JsonSerializer.Serialize(ctoBankRequest, _serializerOptions),
            System.Text.Encoding.UTF8,
            MediaTypeNames.Application.Json);
        var httpResponse = await _httpClient.PostAsync("payments", content);

        var bankResponse = await JsonSerializer.DeserializeAsync<CkoPaymentResponse>(
            await httpResponse.Content.ReadAsStreamAsync(),
            _serializerOptions);

        _logger.LogInformation(
            "Received from Cko Bank payment {@response}",
            new { bankResponse.PaymentId, bankResponse.Status });

        return new Payment(
            bankResponse.PaymentId,
            bankResponse.Approved,
            bankResponse.Status,
            paymentRequest.Reference,
            paymentRequest.Amount,
            paymentRequest.Currency);
    }
}

public class CkoPaymentResponse
{
    public string PaymentId { get; set; } = string.Empty;
    public bool Approved { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CkoPaymentRequest
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

