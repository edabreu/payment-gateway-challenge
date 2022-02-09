using System;
using FluentValidation;

namespace PaymentGateway.API.Dtos.Validators;

public class CardDetailsValidator : AbstractValidator<CardDetails>
{
	public CardDetailsValidator()
	{
		RuleFor(cardDetails => cardDetails.CVV).NotNull().Must(cvv => cvv.Length == 3 && int.TryParse(cvv, out _)).WithMessage("{PropertyName} must be a 3 digit number");
		RuleFor(cardDetails => cardDetails.ExpirationDate).NotNull();
		RuleFor(cardDetails => cardDetails.HolderName).NotNull().NotEmpty();
		RuleFor(cardDetails => cardDetails.Number).NotNull().CreditCard();
	}
}