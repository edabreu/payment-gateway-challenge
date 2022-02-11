using System;
using MongoDB.Bson;

namespace PaymentGateway.API.Dtos.Extensions;

public static class PaymentExtensions
{
    public static Dtos.Payment ToDto(this Domain.Models.Payment payment)
    {
        if (payment is null)
        {
            return default!;
        }

        return new Dtos.Payment
        {
            Amount = payment.Amount,
            Approved = payment.Approved,
            CardToken = payment.CardToken.ToDto(),
            Currency = payment.Currency,
            Id = payment.Id,
            ProcessingId = payment.ProcessingId,
            Merchant = payment.Merchant,
            Reference = payment.Reference,
            Status = payment.Status
        };
    }
}