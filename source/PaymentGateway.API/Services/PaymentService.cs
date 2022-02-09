using System;
using Domain.Abstractions.Gateways;
using PaymentGateway.API.Dtos;

namespace PaymentGateway.API.Services;

public class PaymentService : IPaymentService
{
    private readonly ICkoBankService _bankService;

    public PaymentService(ICkoBankService bankService)
	{
        _bankService = bankService;
    }

    public async Task<PaymentResponse> ProcessAsync(PaymentRequest paymentRequest)
    {
        var payment = await _bankService.ProcessPaymentAsync(new Domain.Models.PaymentRequest(
            paymentRequest.Reference,
            paymentRequest.Amount,
            paymentRequest.Currency,
            new Domain.Models.CardDetails(
                paymentRequest.CardDetails.Number,
                new Domain.Models.ExpirationDate(
                    paymentRequest.CardDetails.ExpirationDate.Month,
                    paymentRequest.CardDetails.ExpirationDate.Year),
                paymentRequest.CardDetails.CVV,
                paymentRequest.CardDetails.HolderName)));

        return new PaymentResponse
        {
            Approved = payment.Approved,
            //CardToken = new CardToken
            //{
            //    HolderName = payment.CardToken.HolderName,
            //    Id = payment.CardToken.Id,
            //    NumberLast4 = payment.CardToken.NumberLast4,
            //    Token = payment.CardToken.Token
            //},
            Id = payment.Id,
            ProcessingId = payment.ProcessingId,
            Reference = payment.Reference,
            Status = payment.Status
        };
    }
}

