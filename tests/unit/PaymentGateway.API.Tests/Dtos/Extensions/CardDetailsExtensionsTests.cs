using System;
using AutoFixture;
using FluentAssertions;
using PaymentGateway.API.Dtos.Extensions;
using Xunit;

namespace PaymentGateway.API.Tests.Dtos.Extensions;

public class CardDetailsExtensionsTests
{
	private readonly Fixture _fixture = new Fixture();

	[Fact]
	public void Mapping_from_dto_should_return_expected_model()
	{
		var source = _fixture.Create<API.Dtos.CardDetails>();

		var result = source.ToModel();

		result.Should().BeOfType<Domain.Models.CardDetails>()
			.And.BeEquivalentTo(source);
	}

	[Fact]
	public void Mapping_from_null_dto_should_return_null_model()
	{
		var source = default(API.Dtos.CardDetails)!;

		var result = source.ToModel();

		result.Should().BeNull();
	}
}