using System;
using AutoFixture;
using Data.Repositories.Dbos.Extensions;
using FluentAssertions;
using Xunit;

namespace Data.Repositories.Tests.Dbos.Extensions;

public class PaymentExtensionsTests
{
	private readonly Fixture _fixture = new Fixture();

	[Fact]
	public void Mapping_from_model_should_return_expected_dbo()
	{
		var source = _fixture.Create<Domain.Models.Payment>();
		source.SetCardToken(_fixture.Create<Domain.Models.CardToken>());

		var result = source.ToDbo();

		result.Should().BeOfType<Data.Repositories.Dbos.Payment>()
			.And.BeEquivalentTo(source, config => config.Excluding(payment => payment.Id));
		result.Id.ToString().Should().Be(source.Id.Substring(0, 24));
	}

	[Fact]
	public void Mapping_from_null_model_should_return_null_dbo()
	{
		var source = default(Domain.Models.Payment)!;

		var result = source.ToDbo();

		result.Should().BeNull();
	}

	[Fact]
	public void Mapping_from_dbo_should_return_expected_model()
	{
		var source = _fixture.Build<Data.Repositories.Dbos.Payment>()
			.With(payment => payment.Id, new MongoDB.Bson.ObjectId(Guid.NewGuid().ToString("N")))
			.Create();

		var result = source.ToModel();

		result.Should().BeOfType<Domain.Models.Payment>()
			.And.BeEquivalentTo(source, config => config.Excluding(payment => payment.Id));
		result.Id.Should().Be(source.Id.ToString());
	}

	[Fact]
	public void Mapping_from_null_dbo_should_return_null_model()
	{
		var source = default(Data.Repositories.Dbos.Payment)!;

		var result = source.ToModel();

		result.Should().BeNull();
	}
}