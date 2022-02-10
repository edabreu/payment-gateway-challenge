using System;

namespace PaymentGateway.API.Dtos;

public class CardToken
{
	public string Id { get; set; } = null!;
	public string Token { get; set; } = null!;
	public string NumberLast4 { get; set; } = null!;
	public string HolderName { get; set; } = null!;
	ExpirationDate ExpirationDate { get; set; } = default!;
}

