using System;

namespace PaymentGateway.API.Dtos;

public class CardToken
{
	public string Id { get; set; }
	public string Token { get; set; }
	public string NumberLast4 { get; set; }
	public string HolderName { get; set; }
	ExpirationDate ExpirationDate { get; set; }
}

