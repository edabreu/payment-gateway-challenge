using System;
using FluentValidation;

namespace PaymentGateway.API.Models.Validators;

public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
{
	public PaymentRequestValidator()
	{
		RuleFor(paymentRequest => paymentRequest.CardDetails).NotNull();
		RuleFor(paymentRequest => paymentRequest.Currency).NotNull().NotEmpty().Length(3);
	}
}