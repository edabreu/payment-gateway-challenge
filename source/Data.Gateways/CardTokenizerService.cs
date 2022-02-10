using System;
using System.Net.Mime;
using System.Text.Json;
using Domain.Abstractions.Gateways;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Data.Gateways;

public class CardTokenizerService : ICardTokenizerService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CardTokenizerService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;

    public CardTokenizerService(HttpClient httpClient, ILogger<CardTokenizerService> logger)
	{
        _httpClient = httpClient;
        _logger = logger;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    public async Task<CardToken> TokenizeCardAsync(CardDetails cardDetails)
    {
        var tokenizeCardRequest = new TokenizeCardRequest
        {
            CVV = cardDetails.CVV,
            HolderName = cardDetails.HolderName,
            Month = cardDetails.ExpirationDate.Month,
            Number = cardDetails.Number,
            Year = cardDetails.ExpirationDate.Year
        };
        var content = new StringContent(
            JsonSerializer.Serialize(tokenizeCardRequest, _serializerOptions),
            System.Text.Encoding.UTF8,
            MediaTypeNames.Application.Json);
        var httpResponse = await _httpClient.PostAsync("tokens", content);

        var tokenizeCardResponse = await JsonSerializer.DeserializeAsync<TokenizeCardResponse>(
            await httpResponse.Content.ReadAsStreamAsync(),
            _serializerOptions);

        _logger.LogInformation("Received card token");

        return new CardToken(
            Id: tokenizeCardResponse.Id,
            Token: tokenizeCardResponse.Token,
            NumberLast4: tokenizeCardResponse.NumberLast4,
            HolderName: tokenizeCardResponse.HolderName,
            ExpirationDate: new ExpirationDate(
                Month: tokenizeCardResponse.Month,
                Year: tokenizeCardResponse.Year));
    }
}

public class TokenizeCardRequest
{
    public string Number { get; set; } = null!;
    public int Month { get; set; }
    public int Year { get; set; }
    public string CVV { get; set; } = null!;
    public string HolderName { get; set; } = null!;
}

public class TokenizeCardResponse
{
    public string Id { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string NumberLast4 { get; set; } = null!;
    public string HolderName { get; set; } = null!;
    public int Month { get; set; }
    public int Year { get; set; }
}