using System;
using AutoFixture;
using FluentAssertions;
using PaymentGateway.API.Dtos.Extensions;
using Xunit;

namespace PaymentGateway.API.Tests.Dtos.Extensions;

public class PaymentRequestExtensionsTests
{
	private readonly Fixture _fixture = new Fixture();

	[Fact]
	public void Mapping_from_dto_with_merchant_should_return_expected_model_with_merchant()
	{
		const string Merchant = "merchant";
		var source = _fixture.Create<API.Dtos.PaymentRequest>();

		var result = source.ToModel(Merchant);

		result.Should().BeOfType<Domain.Models.PaymentRequest>()
			.And.BeEquivalentTo(source);
		result.Merchant.Should().Be(Merchant);
	}

	[Fact]
	public void Mapping_from_dto_should_return_expected_model()
	{
		var source = _fixture.Create<API.Dtos.PaymentRequest>();

		var result = source.ToModel(null!);

		result.Should().BeOfType<Domain.Models.PaymentRequest>()
			.And.BeEquivalentTo(source);
		result.Merchant.Should().BeNull();
	}

	[Theory]
	[InlineData(default(string))]
	[InlineData("a-merchant")]
	public void Mapping_from_null_dto_should_return_null_model(string merchant)
	{
		var source = default(API.Dtos.PaymentRequest)!;

		var result = source.ToModel(merchant);

		result.Should().BeNull();
	}
}

