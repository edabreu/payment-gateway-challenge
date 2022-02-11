namespace PaymentGateway.API.Dtos;

public class Payment
{
    public string Id { get; set; } = null!;
    public string ProcessingId { get; set; } = null!;
    public bool Approved { get; set; }
    public string Status { get; set; } = null!;
    public string Merchant { get; set; } = null!;
    public string Reference { get; set; } = null!;
    public int Amount { get; set; }
    public string Currency { get; set; } = null!;
    public CardToken CardToken { get; set; } = default!;
}
