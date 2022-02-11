using Xunit;

using PaymentGateway.API.Controllers;

using Microsoft.Extensions.Logging;

using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Services;
using PaymentGateway.API.Dtos;
using System.Threading.Tasks;
using System;

namespace PaymentGateway.API.Tests.Controllers;

public class PaymentsControllerTests
{
    private readonly Mock<IPaymentService> _paymentServiceMock = new();
    private readonly Mock<ILogger<PaymentsController>> _loggerMock = new();
    private readonly PaymentsController _controller;

    public PaymentsControllerTests()
    {
        _controller = new PaymentsController(_paymentServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Processing_payment_request_should_return_payment_response()
    {
        const string MerchantId = "merchant";
        var request = new PaymentRequest { Reference = "CKO-Payment-01" };
        var expectedResponse = new Payment();
        _paymentServiceMock.Setup(paymentService => paymentService.ProcessAsync(MerchantId, request))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.ProcessPaymentAsync(MerchantId, request);

        result.Should().BeOfType<CreatedAtActionResult>()
            .Which.Value.Should().BeOfType<Payment>()
            .And.Be(expectedResponse);
        _loggerMock.Verify(logger => logger.Log(
            LogLevel.Information,
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Will process payment request with reference { Reference = CKO-Payment-01 }" && @type.Name == "FormattedLogValues"),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
        Times.Once);
    }

    [Fact]
    public async Task Processing_payment_having_model_validaton_errors_should_return_bad_request()
    {
        const string MerchantId = "merchant";
        var request = new PaymentRequest();
        _controller.ModelState.AddModelError("CardDetails", "CardDetails cannot be null");

        var result = await _controller.ProcessPaymentAsync(MerchantId, request);

        result.Should().BeOfType<BadRequestObjectResult>();
        _paymentServiceMock.Verify(paymentService => paymentService.ProcessAsync(MerchantId, It.IsAny<PaymentRequest>()), Times.Never);
    }

    [Fact]
    public async Task Getting_payment_having_model_validaton_errors_should_return_bad_request()
    {
        _controller.ModelState.AddModelError("merchantId", "merchantId cannot be null");

        var result = await _controller.GetPaymentDetailsAsync(null!, null!);

        result.Should().BeOfType<BadRequestObjectResult>();
        _paymentServiceMock.Verify(paymentService => paymentService.GetAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Getting_existing_payment_matching_merchant_should_return_payment()
    {
        const string MerchantId = "merch-id";
        const string PaymentId = "pay-id";
        var expectedPayment = new Payment { Merchant = MerchantId };
        _paymentServiceMock.Setup(paymentService => paymentService.GetAsync(PaymentId))
            .ReturnsAsync(expectedPayment);

        var result = await _controller.GetPaymentDetailsAsync(MerchantId, PaymentId);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(expectedPayment);
    }

    [Fact]
    public async Task Getting_non_existing_payment_should_return_not_found_result()
    {
        const string MerchantId = "merch-id";
        const string PaymentId = "pay-id";
        _paymentServiceMock.Setup(paymentService => paymentService.GetAsync(PaymentId))
            .ReturnsAsync(default(Payment)!);

        var result = await _controller.GetPaymentDetailsAsync(MerchantId, PaymentId);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Getting_existing_payment_not_matching_merchant_should_return_unauthorized_result()
    {
        const string MerchantId = "merch-id";
        const string PaymentId = "pay-id";
        var expectedPayment = new Payment { Merchant = "other-merch-id" };
        _paymentServiceMock.Setup(paymentService => paymentService.GetAsync(PaymentId))
            .ReturnsAsync(expectedPayment);

        var result = await _controller.GetPaymentDetailsAsync(MerchantId, PaymentId);

        result.Should().BeOfType<UnauthorizedResult>();
    }
}
