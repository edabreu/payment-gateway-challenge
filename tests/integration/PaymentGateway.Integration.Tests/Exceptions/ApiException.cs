using System;
using System.Collections.Generic;
using System.Net;

namespace PaymentGateway.Integration.Tests.Exceptions;

public class ApiException : Exception
{
    public int Status { get; set; }

    public override string Message => $"Server responded with {Status} http status codes";
}