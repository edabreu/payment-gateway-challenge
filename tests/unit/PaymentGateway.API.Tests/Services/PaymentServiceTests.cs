using System;
using System.Threading.Tasks;
using AutoFixture;
using Domain.Abstractions.Gateways;
using Domain.Abstractions.Repositories;
using FluentAssertions;
using Moq;
using PaymentGateway.API.Dtos.Extensions;
using PaymentGateway.API.Services;
using Xunit;

namespace PaymentGateway.API.Tests.Services;

public class PaymentServiceTests
{
	private readonly Mock<ICardTokenizerService> _cardTokenizerServiceMock = new();
	private readonly Mock<ICkoBankService> _bankServiceMock = new();
	private readonly Mock<IPaymentsRepository> _paymentsRepositoryMock = new();
	private readonly IPaymentService _paymentService;
	private readonly Fixture _fixture = new Fixture();

	public PaymentServiceTests()
	{
		_paymentService = new PaymentService(
			_cardTokenizerServiceMock.Object,
			_bankServiceMock.Object,
			_paymentsRepositoryMock.Object);
	}

	[Fact]
	public async Task Processing_payment_should_return_payment_result()
	{
		const string Merchant = "merchant";
		var request = _fixture.Create<API.Dtos.PaymentRequest>();
		var expectedBankPayment = _fixture.Create<Domain.Models.Payment>();
		Domain.Models.PaymentRequest paymentRequestModel = default!;
		_bankServiceMock.Setup(bankService => bankService.ProcessPaymentAsync(It.IsAny<Domain.Models.PaymentRequest>()))
			.Callback<Domain.Models.PaymentRequest>(req => { paymentRequestModel = req; })
			.ReturnsAsync(expectedBankPayment);
		var expectedTokenizerCardToken = _fixture.Create<Domain.Models.CardToken>();
		Domain.Models.CardDetails tokenizerCardDetailsRequest = default!;
		_cardTokenizerServiceMock.Setup(
			tokenizerService => tokenizerService.TokenizeCardAsync(It.IsAny<Domain.Models.CardDetails>()))
			.Callback<Domain.Models.CardDetails>(req => { tokenizerCardDetailsRequest = req; })
			.ReturnsAsync(expectedTokenizerCardToken);
		Domain.Models.Payment persistedPayment = default!;
		_paymentsRepositoryMock.Setup(repository => repository.InsertAsync(It.IsAny<Domain.Models.Payment>()))
			.Callback<Domain.Models.Payment>(payment => { persistedPayment = payment; })
			.ReturnsAsync(expectedBankPayment);

		var result = await _paymentService.ProcessAsync(Merchant, request);

		paymentRequestModel.Should().BeEquivalentTo(request);
		tokenizerCardDetailsRequest.Should().BeEquivalentTo(request.CardDetails);
		expectedBankPayment.CardToken.Should().NotBeNull()
			.And.Be(expectedTokenizerCardToken);
		persistedPayment.Should().Be(expectedBankPayment);
		result.Should().BeEquivalentTo(expectedBankPayment);
	}

	[Fact]
	public async Task Getting_payment_by_id_should_return_payment()
	{
		const string paymentId = "pay-id";
		var expectedResult = _fixture.Create<Domain.Models.Payment>();
		_paymentsRepositoryMock.Setup(repository => repository.GetAsync(paymentId))
			.ReturnsAsync(expectedResult);

		var result = await _paymentService.GetAsync(paymentId);

		result.Should().BeEquivalentTo(expectedResult.ToDto());
	}

	[Fact]
	public async Task Getting_non_existing_payment_by_id_should_return_null()
	{
		const string paymentId = "pay-id";
		_paymentsRepositoryMock.Setup(repository => repository.GetAsync(paymentId))
			.ReturnsAsync(default(Domain.Models.Payment));

		var result = await _paymentService.GetAsync(paymentId);

		result.Should().BeNull();
	}
}
