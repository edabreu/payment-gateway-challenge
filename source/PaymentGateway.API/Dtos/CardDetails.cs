namespace PaymentGateway.API.Dtos;

public class CardDetails
{
    public string Number { get; set; } = null!;
    public ExpirationDate ExpirationDate { get; set; } = default!;
    public string CVV { get; set; } = null!;
    public string HolderName { get; set; } = null!;
}
