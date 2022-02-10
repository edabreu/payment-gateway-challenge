using System;
using MongoDB.Bson;

namespace Data.Repositories.Dbos.Extensions;

public static class PaymentExtensions
{
    public static Dbos.Payment ToDbo(this Domain.Models.Payment payment)
    {
        return new Dbos.Payment
        {
            Amount = payment.Amount,
            Approved = payment.Approved,
            CardToken = payment.CardToken?.ToDbo(),
            Currency = payment.Currency,
            Id = new ObjectId(payment.Id ?? Guid.NewGuid().ToString("N")),
            ProcessingId = payment.ProcessingId,
            Reference = payment.Reference,
            Status = payment.Status
        };
    }

    public static Domain.Models.Payment ToModel(this Dbos.Payment payment)
    {
        return new Domain.Models.Payment(
            id: payment.Id.ToString(),
            processingId: payment.ProcessingId,
            approved: payment.Approved,
            status: payment.Status,
            reference: payment.Reference,
            amount: payment.Amount,
            currecy: payment.Currency,
            cardToken: payment.CardToken?.ToModel());
    }
}