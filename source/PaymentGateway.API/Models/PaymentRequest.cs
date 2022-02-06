namespace PaymentGateway.API.Models;

public class PaymentRequest
{
    public string Reference { get; set; }
    public int Amount { get; set; }
    public string Currency { get; set; }
    public CardDetails CardDetails { get; set; }
}
