using System;

namespace Domain.Models;

/// <summary>
/// The credit card token
/// </summary>
/// <param name="Id">Token id</param>
/// <param name="Token">Token string</param>
/// <param name="NumberLast4">Credit card last 4 digits</param>
/// <param name="HolderName">Credit card holder name</param>
/// <param name="ExpirationDate">Credit card expiration date</param>
public record CardToken(string Id, string Token, string NumberLast4, string HolderName, ExpirationDate ExpirationDate);

