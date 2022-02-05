using Xunit;

using PaymentGateway.API.Controllers;

using Microsoft.Extensions.Logging;

using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Services;

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
    public void Processing_payment_request_should_return_payment_response()
    {
        var request = new PaymentRequest();
        var expectedResponse = new PaymentResponse();
        _paymentServiceMock.Setup(paymentService => paymentService.ProcessAsync(request))
            .ReturnsAsync(expectedResponse);

        var result = _controller.ProcessPaymentAsync(request);

        result.Should().BeOfType<ActionResult<PaymentResponse>>()
            .Which.Value.Should().Be(expectedResponse);
    }
}
