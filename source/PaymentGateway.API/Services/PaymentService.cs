using System;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Services;

public class PaymentService : IPaymentService
{
	public PaymentService()
	{
	}

    public Task<PaymentResponse> ProcessAsync(PaymentRequest paymentRequest)
    {
        return Task.FromResult(new PaymentResponse());
    }
}

