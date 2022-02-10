using System;
using Domain.Abstractions.Gateways;
using Domain.Abstractions.Repositories;
using PaymentGateway.API.Dtos;

namespace PaymentGateway.API.Services;

public class PaymentService : IPaymentService
{
    private readonly ICardTokenizerService _cardTokenizerService;
    private readonly ICkoBankService _bankService;
    private readonly IPaymentsRepository _paymentsRepository;

    public PaymentService(
        ICardTokenizerService cardTokenizerService,
        ICkoBankService bankService,
        IPaymentsRepository paymentsRepository)
	{
        _cardTokenizerService = cardTokenizerService;
        _bankService = bankService;
        _paymentsRepository = paymentsRepository;
    }

    public async Task<PaymentResponse> ProcessAsync(PaymentRequest paymentRequest)
    {
        var request = new Domain.Models.PaymentRequest(
            paymentRequest.Reference,
            paymentRequest.Amount,
            paymentRequest.Currency,
            new Domain.Models.CardDetails(
                paymentRequest.CardDetails.Number,
                new Domain.Models.ExpirationDate(
                    paymentRequest.CardDetails.ExpirationDate.Month,
                    paymentRequest.CardDetails.ExpirationDate.Year),
                paymentRequest.CardDetails.CVV,
                paymentRequest.CardDetails.HolderName));

        var paymentTask = _bankService.ProcessPaymentAsync(request);
        var cardTokenizerTask = _cardTokenizerService.TokenizeCardAsync(request.CardDetails);

        await Task.WhenAll(paymentTask, cardTokenizerTask);
        var payment = paymentTask.Result;
        payment.SetCardToken(cardTokenizerTask.Result);

        await _paymentsRepository.InsertAsync(payment);

        return new PaymentResponse
        {
            Approved = payment.Approved,
            CardToken = new CardToken
            {
                HolderName = payment.CardToken.HolderName,
                Id = payment.CardToken.Id,
                NumberLast4 = payment.CardToken.NumberLast4,
                Token = payment.CardToken.Token
            },
            Id = payment.Id,
            ProcessingId = payment.ProcessingId,
            Reference = payment.Reference,
            Status = payment.Status
        };
    }
}

