using System;
using PaymentGateway.API.Controllers;

namespace PaymentGateway.API.Services
{
	public interface IPaymentService
	{
		Task<PaymentResponse> ProcessAsync(PaymentRequest paymentRequest);
	}
}

