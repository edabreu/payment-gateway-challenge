using System;
using System.Net.Http;
using System.Text.Json;

namespace payment_gateway_challenge.Integration.Tests;

public class TestFixture : IDisposable
{
    private readonly PlaygroundApplication _application;
    private bool _disposedValue;

    public TestFixture()
	{
        _application = new();
        HttpClient = _application.CreateClient();
        SerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    public IServiceProvider ServiceProvider => _application.Services;
    public HttpClient HttpClient { get; }
    public JsonSerializerOptions SerializerOptions { get; }

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