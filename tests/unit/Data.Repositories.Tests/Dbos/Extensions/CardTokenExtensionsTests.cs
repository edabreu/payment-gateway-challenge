using System;
using AutoFixture;
using Data.Repositories.Dbos.Extensions;
using FluentAssertions;
using Xunit;

namespace Data.Repositories.Tests.Dbos.Extensions;

public class CardTokenExtensionsTests
{
	private readonly Fixture _fixture = new Fixture();

	[Fact]
	public void Mapping_from_model_should_return_expected_dbo()
	{
		var source = _fixture.Create<Domain.Models.CardToken>();

		var result = source.ToDbo();

		result.Should().BeOfType<Data.Repositories.Dbos.CardToken>()
			.And.BeEquivalentTo(source);
	}

	[Fact]
	public void Mapping_from_null_model_should_return_null_dbo()
	{
		var source = default(Domain.Models.CardToken)!;

		var result = source.ToDbo();

		result.Should().BeNull();
	}

	[Fact]
	public void Mapping_from_dbo_should_return_expected_model()
	{
		var source = _fixture.Create<Data.Repositories.Dbos.CardToken>();

		var result = source.ToModel();

		result.Should().BeOfType<Domain.Models.CardToken>()
			.And.BeEquivalentTo(source);
	}

	[Fact]
	public void Mapping_from_null_dbo_should_return_null_model()
	{
		var source = default(Data.Repositories.Dbos.CardToken)!;

		var result = source.ToModel();

		result.Should().BeNull();
	}
}