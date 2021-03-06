using System;

namespace Data.Repositories.Dbos.Extensions;

public static class ExpirationDateExtensions
{
    public static Dbos.ExpirationDate ToDbo(this Domain.Models.ExpirationDate expirationDate)
    {
        if (expirationDate is null)
        {
            return default!;
        }

        return new Dbos.ExpirationDate
        {
            Month = expirationDate.Month,
            Year = expirationDate.Year
        };
    }

    public static Domain.Models.ExpirationDate ToModel(this Dbos.ExpirationDate expirationDate)
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
