using System;
using PaymentGateway.API.Controllers;
using PaymentGateway.API.Dtos;

namespace PaymentGateway.API.Services
{
	public interface IPaymentService
	{
		Task<Payment> ProcessAsync(string merchant, PaymentRequest paymentRequest);
		Task<Payment> GetAsync(string id);
	}
}

