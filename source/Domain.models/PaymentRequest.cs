using System;

namespace Domain.Models;

public record PaymentRequest(string Reference, int Amount, string Currency, CardDetails CardDetails);

