using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace payment_gateway_challenge.Integration.Tests;

public class PlaygroundApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices((context, services) =>
        {
            // cenas
        });

        return base.CreateHost(builder);
    }
}
