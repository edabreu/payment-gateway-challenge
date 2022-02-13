using System;
using AutoFixture;
using FluentAssertions;
using PaymentGateway.API.Dtos.Extensions;
using Xunit;

namespace PaymentGateway.API.Tests.Dtos.Extensions;

public class PaymentExtensionsTests
{
	private readonly Fixture _fixture = new Fixture();

	[Fact]
	public void Mapping_from_model_should_return_expected_dto()
	{
		var source = _fixture.Create<Domain.Models.Payment>();

		var result = source.ToDto();

		result.Should().BeOfType<API.Dtos.Payment>()
			.And.BeEquivalentTo(source);
	}

	[Fact]
	public void Mapping_from_null_model_should_return_null_dto()
	{
		var source = default(Domain.Models.Payment)!;

		var result = source.ToDto();

		result.Should().BeNull();
	}
}