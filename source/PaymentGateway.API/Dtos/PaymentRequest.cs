namespace PaymentGateway.API.Dtos;

public class PaymentRequest
{
    public string Reference { get; set; } = null!;
    public int Amount { get; set; }
    public string Currency { get; set; } = null!;
    public CardDetails? CardDetails { get; set; } = default!;
}
