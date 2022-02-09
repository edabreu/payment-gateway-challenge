namespace PaymentGateway.API.Dtos;

public class CardDetails
{
    public string Number { get; set; } = String.Empty;
    public ExpirationDate? ExpirationDate { get; set; }
    public string CVV { get; set; } = String.Empty;
    public string HolderName { get; set; } = String.Empty;
}
