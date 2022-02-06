namespace PaymentGateway.API.Models;

public class CardDetails
{
    public string PrimaryAccountNumber { get; set; }
    public ExpirationDate expirationDate { get; set; }
    public string CVV { get; set; }
}
