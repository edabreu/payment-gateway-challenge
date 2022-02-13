using System;
using FluentValidation;

namespace PaymentGateway.API.Dtos.Validators;

public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
{
	public PaymentRequestValidator()
	{
		RuleFor(paymentRequest => paymentRequest.Amount).GreaterThanOrEqualTo(0);
		RuleFor(paymentRequest => paymentRequest.CardDetails).NotNull();
		RuleFor(paymentRequest => paymentRequest.Currency).NotNull().Length(3);
		RuleFor(paymentRequest => paymentRequest.Reference).NotNull().MaximumLength(30);
	}
}