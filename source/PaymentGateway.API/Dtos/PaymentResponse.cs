namespace PaymentGateway.API.Dtos;

public class PaymentResponse
{
    public string Id { get; set; }
    public string ProcessingId { get; set; }
    public bool Approved { get; set; }
    public string Status { get; set; }
    public string Reference { get; set; }
    public CardToken? CardToken { get; set; }
}
