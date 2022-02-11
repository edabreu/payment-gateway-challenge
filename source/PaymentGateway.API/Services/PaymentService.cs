using System;
using Domain.Abstractions.Gateways;
using Domain.Abstractions.Repositories;
using PaymentGateway.API.Dtos;
using PaymentGateway.API.Dtos.Extensions;

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

    public async Task<Payment> ProcessAsync(string merchant, PaymentRequest paymentRequest)
    {
        var request = paymentRequest.ToModel(merchant);
        var paymentTask = _bankService.ProcessPaymentAsync(request);
        var cardTokenizerTask = _cardTokenizerService.TokenizeCardAsync(request.CardDetails);

        await Task.WhenAll(paymentTask, cardTokenizerTask);
        var payment = paymentTask.Result;
        payment.SetCardToken(cardTokenizerTask.Result);

        payment = await _paymentsRepository.InsertAsync(payment);

        return payment.ToDto();
    }

    public async Task<Payment> GetAsync(string id)
    {
        var payment = await _paymentsRepository.GetAsync(id);
        return payment.ToDto();
    }
}

