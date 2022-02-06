using Xunit;

using PaymentGateway.API.Controllers;

using Microsoft.Extensions.Logging;

using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Services;
using PaymentGateway.API.Models;
using System.Threading.Tasks;

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
        var request = new PaymentRequest();
        var expectedResponse = new PaymentResponse();
        _paymentServiceMock.Setup(paymentService => paymentService.ProcessAsync(request))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.ProcessPaymentAsync(request);

        result.Should().BeOfType<ActionResult<PaymentResponse>>()
            .Which.Value.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task Processing_invalid_payment_request_should_return_bad_request()
    {
        var request = new PaymentRequest();
        _controller.ModelState.AddModelError("CardDetails", "CardDetails cannot be null");

        var result = await _controller.ProcessPaymentAsync(request);

        result.Should().BeOfType<ActionResult<PaymentResponse>>()
            .Which.Result.Should().BeOfType<BadRequestObjectResult>();
        _paymentServiceMock.Verify(paymentService => paymentService.ProcessAsync(It.IsAny<PaymentRequest>()), Times.Never);
    }
}
