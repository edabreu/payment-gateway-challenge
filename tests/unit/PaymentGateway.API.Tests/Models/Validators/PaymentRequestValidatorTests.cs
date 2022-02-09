using System;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using PaymentGateway.API.Dtos;
using PaymentGateway.API.Dtos.Validators;
using Xunit;

namespace PaymentGateway.API.Tests.Models.Validators;

public class PaymentRequestValidatorTests
{
	private readonly PaymentRequestValidator _validator;

	public PaymentRequestValidatorTests()
	{
		_validator = new PaymentRequestValidator();
	}

	[Fact]
	public void Should_have_error_when_currency_is_null()
	{
		var paymentRequest = new PaymentRequest();

		var result = _validator.TestValidate(paymentRequest)!;

		result.ShouldHaveValidationErrorFor(model => model.CardDetails).WithErrorMessage("'Card Details' must not be empty.");
		result.ShouldHaveValidationErrorFor(model => model.Currency).WithErrorMessage("'Currency' must be 3 characters in length. You entered 0 characters.");
		result.ShouldHaveValidationErrorFor(model => model.Reference);
	}

	[Fact]
	public void Should_have_error_when_amount_is_less_than_zero()
	{
		var paymentRequest = new PaymentRequest
		{
			Amount = -1,
			CardDetails = new CardDetails(),
			Currency = "EUR",
			Reference = "Reference"
		};

		var result = _validator.TestValidate(paymentRequest)!;

		result.ShouldHaveValidationErrorFor(model => model.Amount).WithErrorMessage("'Amount' must be greater than or equal to '0'.");
	}

	[Fact]
	public void Should_have_error_when_currency_length_is_not_three()
	{
		var paymentRequest = new PaymentRequest
		{
			CardDetails = new CardDetails(),
			Currency = "EU",
			Reference = "Reference"
		};

		var result = _validator.TestValidate(paymentRequest)!;

		result.ShouldHaveValidationErrorFor(model => model.Currency);
	}
}

