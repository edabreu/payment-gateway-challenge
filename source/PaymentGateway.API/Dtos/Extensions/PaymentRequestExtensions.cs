using System;

namespace PaymentGateway.API.Dtos.Extensions;

public static class PaymentRequestExtensions
{
	public static Domain.Models.PaymentRequest ToModel(this PaymentRequest request, string merchant)
	{
		return new Domain.Models.PaymentRequest(
			Merchant: merchant,
			Reference: request.Reference,
			Amount: request.Amount,
			Currency: request.Currency,
			CardDetails: request.CardDetails.ToModel());
	}
}
