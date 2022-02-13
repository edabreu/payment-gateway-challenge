using System;
using System.Net.Http;
using System.Text.Json;

namespace PaymentGateway.Integration.Tests;

public class TestFixture : IDisposable
{
    private readonly PlaygroundApplication _application;
    private bool _disposedValue;

    public TestFixture()
	{
        _application = new();
        HttpClient = _application.CreateClient();
    }

    public IServiceProvider ServiceProvider => _application.Services;
    public HttpClient HttpClient { get; }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _application.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}