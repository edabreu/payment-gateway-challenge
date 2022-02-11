using System;

namespace PaymentGateway.API.Dtos.Extensions;

public static class ExpirationDateExtensions
{
    public static Dtos.ExpirationDate ToDto(this Domain.Models.ExpirationDate expirationDate)
    {
        if (expirationDate is null)
        {
            return default!;
        }

        return new Dtos.ExpirationDate
        {
            Month = expirationDate.Month,
            Year = expirationDate.Year
        };
    }

    public static Domain.Models.ExpirationDate ToModel(this Dtos.ExpirationDate expirationDate)
    {
        if (expirationDate is null)
        {
            return default!;
        }

        return new Domain.Models.ExpirationDate(
            Month: expirationDate.Month,
            Year: expirationDate.Year);
    }
}
