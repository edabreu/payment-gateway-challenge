using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using FluentAssertions;
using PaymentGateway.API.Dtos;
using Xunit;
using Xunit.Abstractions;

namespace payment_gateway_challenge.Integration.Tests;

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
                HolderName = "Eduardo Abreu",
                ExpirationDate = new()
                {
                    Month = 12,
                    Year = 2022
                }
            }
        };

        var payment = await GetPaymentAsync(MerchantId, PaymentId);

        payment.Should().BeEquivalentTo(expectedResult);
    }

    private async Task<Payment> GetPaymentAsync(string merchantId, string paymentId)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"payments/{paymentId}");
        httpRequest.Headers.Add("X-merchant_id", merchantId);
        var httpResponse = await _fixture.HttpClient.SendAsync(httpRequest);

        if (!httpResponse.IsSuccessStatusCode)
        {
        }

        return await JsonSerializer.DeserializeAsync<Payment>(
            await httpResponse.Content.ReadAsStreamAsync(),
            _fixture.SerializerOptions);
    }
}