using System;
using AutoFixture;
using FluentAssertions;
using PaymentGateway.API.Dtos.Extensions;
using Xunit;

namespace PaymentGateway.API.Tests.Dtos.Extensions;

public class ExpirationDateExtensionsTests
{
	private readonly Fixture _fixture = new Fixture();

	[Fact]
	public void Mapping_from_dto_should_return_expected_model()
	{
		var source = _fixture.Create<API.Dtos.ExpirationDate>();

		var result = source.ToModel();

		result.Should().BeOfType<Domain.Models.ExpirationDate>()
			.And.BeEquivalentTo(source);
	}

	[Fact]
	public void Mapping_from_null_dto_should_return_null_model()
	{
		var source = default(API.Dtos.ExpirationDate)!;

		var result = source.ToModel();

		result.Should().BeNull();
	}

	[Fact]
	public void Mapping_from_model_should_return_expected_dto()
	{
		var source = _fixture.Create<Domain.Models.ExpirationDate>();

		var result = source.ToDto();

		result.Should().BeOfType<API.Dtos.ExpirationDate>()
			.And.BeEquivalentTo(source);
	}

	[Fact]
	public void Mapping_from_null_model_should_return_null_dto()
	{
		var source = default(Domain.Models.ExpirationDate)!;

		var result = source.ToDto();

		result.Should().BeNull();
	}
}