using System;
using PaymentGateway.API.Controllers;
using PaymentGateway.API.Dtos;

namespace PaymentGateway.API.Services
{
	public interface IPaymentService
	{
		Task<PaymentResponse> ProcessAsync(PaymentRequest paymentRequest);
	}
}

