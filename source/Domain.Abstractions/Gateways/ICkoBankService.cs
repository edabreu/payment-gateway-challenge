using Domain.Models;

namespace Domain.Abstractions.Gateways;

public interface ICkoBankService
{
    Task<Payment> ProcessPaymentAsync(PaymentRequest paymentRequest);
}