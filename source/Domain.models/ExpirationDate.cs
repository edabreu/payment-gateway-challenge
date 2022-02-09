using System;

namespace Domain.Models;

/// <summary>
/// The credit card expiration date
/// </summary>
/// <param name="Month">Month</param>
/// <param name="Year">Year</param>
public record ExpirationDate(int Month, int Year);

