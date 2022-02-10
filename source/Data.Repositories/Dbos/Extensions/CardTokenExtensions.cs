using System;

namespace Data.Repositories.Dbos.Extensions;

public static class CardTokenExtensions
{
    public static Dbos.CardToken ToDbo(this Domain.Models.CardToken cardToken)
    {
        return new Dbos.CardToken
        {
            ExpirationDate = cardToken.ExpirationDate.ToDbo(),
            HolderName = cardToken.HolderName,
            Id = cardToken.Id,
            NumberLast4 = cardToken.NumberLast4,
            Token = cardToken.Token
        };
    }

    public static Domain.Models.CardToken ToModel(this Dbos.CardToken cardToken)
    {
        return new Domain.Models.CardToken(
            Id: cardToken.Id,
            Token: cardToken.Token,
            NumberLast4: cardToken.NumberLast4,
            HolderName: cardToken.HolderName,
            ExpirationDate: cardToken.ExpirationDate.ToModel());
    }
}