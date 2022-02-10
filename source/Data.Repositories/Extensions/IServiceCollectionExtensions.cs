using System;
using Data.Repositories;
using Data.Repositories.Options;
using Domain.Abstractions.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
	public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
	{
		return services.Configure<MongoOptions>(configuration.GetSection(nameof(MongoOptions)))
			.AddSingleton<IMongoClient>(
				provider => new MongoClient(provider.GetService<IOptions<MongoOptions>>()!.Value.ConnectionString))
			.AddScoped<IPaymentsRepository, PaymentsRepository>();
	}
}
