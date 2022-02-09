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

namespace PaymentGateway.API.Tests;

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
        var expectedResponse = new PaymentResponse();
        _paymentServiceMock.Setup(paymentService => paymentService.ProcessAsync(request))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.ProcessPaymentAsync(MerchantId, request);

        result.Should().BeOfType<ActionResult<PaymentResponse>>()
            .Which.Result.Should().BeOfType<CreatedAtActionResult>()
            .Which.Value.Should().Be(expectedResponse);
        _loggerMock.Verify(logger => logger.Log(
            LogLevel.Information,
            It.Is<EventId>(eventId => eventId.Id == 0),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Will process payment request with reference { Reference = CKO-Payment-01 }" && @type.Name == "FormattedLogValues"),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
        Times.Once);
    }

    [Fact]
    public async Task Processing_invalid_payment_request_should_return_bad_request()
    {
        const string MerchantId = "merchant";
        var request = new PaymentRequest();
        _controller.ModelState.AddModelError("CardDetails", "CardDetails cannot be null");

        var result = await _controller.ProcessPaymentAsync(MerchantId, request);

        result.Should().BeOfType<ActionResult<PaymentResponse>>()
            .Which.Result.Should().BeOfType<BadRequestObjectResult>();
        _paymentServiceMock.Verify(paymentService => paymentService.ProcessAsync(It.IsAny<PaymentRequest>()), Times.Never);
    }
}
