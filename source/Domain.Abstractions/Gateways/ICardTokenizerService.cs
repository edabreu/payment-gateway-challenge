using Domain.Models;

namespace Domain.Abstractions.Gateways;

public interface ICardTokenizerService
{
    Task<CardToken> TokenizeCardAsync(CardDetails cardDetails);
}