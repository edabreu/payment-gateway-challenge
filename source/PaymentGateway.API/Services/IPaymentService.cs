using System;
using PaymentGateway.API.Controllers;
using PaymentGateway.API.Models;

namespace PaymentGateway.API.Services
{
	public interface IPaymentService
	{
		Task<PaymentResponse> ProcessAsync(PaymentRequest paymentRequest);
	}
}

