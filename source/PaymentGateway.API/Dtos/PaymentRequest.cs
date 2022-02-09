namespace PaymentGateway.API.Dtos;

public class PaymentRequest
{
    public string Reference { get; set; } = string.Empty;
    public int Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public CardDetails? CardDetails { get; set; }
}
