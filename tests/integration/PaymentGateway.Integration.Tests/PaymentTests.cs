using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using PaymentGateway.API.Dtos;
using Xunit;
using Xunit.Abstractions;
using System;
using PaymentGateway.Integration.Tests.Exceptions;
using PaymentGateway.Integration.Tests.Extensions;

namespace PaymentGateway.Integration.Tests;

public class PaymentTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public PaymentTests(TestFixture fixture, ITestOutputHelper testOutputHelper)
	{
        this._fixture = fixture;
        this._testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task Get_existing_payment_id_should_return_expected_payment()
    {
        const string MerchantId = "merch-1234-2341";
        const string PaymentId = "00eecb99b41848a28b6434bf";
        Payment expectedResult = new()
        {
            Id = "00eecb99b41848a28b6434bf",
            ProcessingId = "24bdb51d-c5f5-4c3b-8581-5026b987aa7a",
            Approved = true,
            Status = "Authorized",
            Merchant = "merch-1234-2341",
            Reference = "pay-202202-123456",
            Amount = 23550,
            Currency = "eur",
            CardToken = new()
            {
                Id = "a93b50de30864886b4c937efdf4210df",
                Token = "A1ADFEF4B97D3E73986CAC01F24F905429F17D4A23C401877D3DB18145888EA3",
                NumberLast4 = "7271",
                HolderName = "John Doe",
                ExpirationDate = new()
                {
                    Month = 12,
                    Year = 2022
                }
            }
        };

        var payment = await _fixture.HttpClient.SendAsync<Payment>(
            HttpMethod.Get,
            $"payments/{PaymentId}",
            request =>
            {
                request.Headers.Add("X-merchant_id", MerchantId);
            });

        payment.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task Get_non_payment_id_should_return_not_found_http_status_code()
    {
        const string MerchantId = "merch-1234-2341";
        const string PaymentId = "00eecb99b41848a28b6434aa";

        Func<Task> getPaymentIntent = async () => await _fixture.HttpClient.SendAsync<Payment>(
            HttpMethod.Get,
            $"payments/{PaymentId}",
            request =>
            {
                request.Headers.Add("X-merchant_id", MerchantId);
            });

        var exceptionAssertion = await getPaymentIntent.Should().ThrowAsync<ApiException>();
        exceptionAssertion.Which.Status.Should().Be((int)System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_non_payment_id_should_return_unauthorized_http_status_code()
    {
        const string MerchantId = "other-merch";
        const string PaymentId = "00eecb99b41848a28b6434bf";

        Func<Task> getPaymentIntent = async () => await _fixture.HttpClient.SendAsync<Payment>(
            HttpMethod.Get,
            $"payments/{PaymentId}",
            request =>
            {
                request.Headers.Add("X-merchant_id", MerchantId);
            });

        var exceptionAssertion = await getPaymentIntent.Should().ThrowAsync<ApiException>();
        exceptionAssertion.Which.Status.Should().Be((int)System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Processing_payment_request_should_return_payment_details()
    {
        const string MerchantId = "merch-1234-1111";
        PaymentRequest request = new()
        {
            Reference = $"p-202202-{DateTime.Now.Ticks}",
            Amount = 1235,
            Currency = "EUR",
            CardDetails = new()
            {
                Number = "1234567890123452",
                ExpirationDate = new()
                {
                    Month = 12,
                    Year = 2023
                },
                CVV = "312",
                HolderName = "John Doe Smith"
            }
        };
        var expectedPayment = new
        {
            Amount = request.Amount,
            Approved = true,
            CardToken = new
            {
                ExpirationDate = new
                {
                    Month = request.CardDetails.ExpirationDate.Month,
                    Year = request.CardDetails.ExpirationDate.Year
                },
                HolderName = request.CardDetails.HolderName,
                NumberLast4 = request.CardDetails.Number.Substring(request.CardDetails.Number.Length - 4),
            },
            Currency = request.Currency,
            Merchant = MerchantId,
            Reference = request.Reference,
            Status = "Authorized"
        };

        var payment = await _fixture.HttpClient.SendAsync<PaymentRequest, Payment>(
            HttpMethod.Post,
            "payments",
            request,
            httpRequest =>
            {
                httpRequest.Headers.Add("X-merchant_id", MerchantId);

            });

        payment.Should().BeEquivalentTo(expectedPayment);
    }

    [Fact]
    public async Task Processing_payment_request_using_card_with_insufficient_funds_should_return_payment_details()
    {
        const string MerchantId = "merch-9999-1111";
        PaymentRequest request = new()
        {
            Reference = $"p-202202-{DateTime.Now.Ticks}",
            Amount = 1235,
            Currency = "USD",
            CardDetails = new()
            {
                Number = "4643335951477921",
                ExpirationDate = new()
                {
                    Month = 12,
                    Year = 2023
                },
                CVV = "312",
                HolderName = "John Doe Smith"
            }
        };
        var expectedPayment = new
        {
            Amount = request.Amount,
            Approved = false,
            CardToken = new
            {
                ExpirationDate = new
                {
                    Month = request.CardDetails.ExpirationDate.Month,
                    Year = request.CardDetails.ExpirationDate.Year
                },
                HolderName = request.CardDetails.HolderName,
                NumberLast4 = request.CardDetails.Number.Substring(request.CardDetails.Number.Length - 4),
            },
            Currency = request.Currency,
            Merchant = MerchantId,
            Reference = request.Reference,
            Status = "InsuficientFunds"
        };

        var payment = await _fixture.HttpClient.SendAsync<PaymentRequest, Payment>(
            HttpMethod.Post,
            "payments",
            request,
            httpRequest =>
            {
                httpRequest.Headers.Add("X-merchant_id", MerchantId);

            });

        payment.Should().BeEquivalentTo(expectedPayment);
    }

    [Fact]
    public async Task Processing_invalid_payment_request_should_return_bad_request()
    {
        const string MerchantId = "merch-1234-1111";
        PaymentRequest request = new()
        {
            Reference = $"p-202202-{DateTime.Now.Ticks}",
            Amount = 123456,
            Currency = "EUR9",
            CardDetails = new()
            {
                Number = "1234567890123452",
                ExpirationDate = new()
                {
                    Month = 12,
                    Year = 2023
                },
                CVV = "312",
                HolderName = "John Doe Smith"
            }
        };

        Func<Task> processPaymentIntent = async () => await _fixture.HttpClient.SendAsync<PaymentRequest, Payment>(
            HttpMethod.Post,
            "payments",
            request,
            httpRequest =>
            {
                httpRequest.Headers.Add("X-merchant_id", MerchantId);

            });

        var exceptionAssertion = await processPaymentIntent.Should().ThrowAsync<ApiException>();
        exceptionAssertion.Which.Status.Should().Be((int)System.Net.HttpStatusCode.BadRequest);
    }
}