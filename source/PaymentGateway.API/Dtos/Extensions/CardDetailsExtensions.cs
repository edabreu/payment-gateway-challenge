using System;

namespace PaymentGateway.API.Dtos.Extensions;

public static class CardDetailsExtensions
{
	public static Domain.Models.CardDetails ToModel(this CardDetails cardDetails)
	{
		if (cardDetails is null)
		{
			return default(Domain.Models.CardDetails)!;
		}

		return new Domain.Models.CardDetails(
			Number: cardDetails.Number,
			ExpirationDate: cardDetails.ExpirationDate.ToModel(),
			CVV: cardDetails.CVV,
			HolderName: cardDetails.HolderName);
	}
}
