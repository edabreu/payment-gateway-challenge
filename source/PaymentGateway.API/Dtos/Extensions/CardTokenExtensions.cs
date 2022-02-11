using System;

namespace PaymentGateway.API.Dtos.Extensions;

public static class CardTokenExtensions
{
    public static Dtos.CardToken ToDto(this Domain.Models.CardToken cardToken)
    {
        if (cardToken is null)
        {
            return default!;
        }

        return new Dtos.CardToken
        {
            ExpirationDate = cardToken.ExpirationDate.ToDto(),
            HolderName = cardToken.HolderName,
            Id = cardToken.Id,
            NumberLast4 = cardToken.NumberLast4,
            Token = cardToken.Token
        };
    }
}