using System;
using Domain.Abstractions.Gateways;
using Data.Gateways;
using Data.Gateways.Options;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddGateways(this IServiceCollection services, IConfiguration configuration)
    {
        var gateways = new GatewayOptions();
        configuration.GetSection(nameof(GatewayOptions)).Bind(gateways);
        services.AddHttpClient<ICkoBankService, CkoBankService>(client =>
        {
            client.BaseAddress = new Uri(gateways.CkoBank.Endpoint);
        });
        services.AddHttpClient<ICardTokenizerService, CardTokenizerService>(client =>
        {
            client.BaseAddress = new Uri(gateways.CardTokenizer.Endpoint);
        });

        return services;
    }
}