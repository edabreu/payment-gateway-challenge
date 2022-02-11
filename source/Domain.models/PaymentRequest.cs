using System;

namespace Domain.Models;

public record PaymentRequest(string Merchant, string Reference, int Amount, string Currency, CardDetails CardDetails);

