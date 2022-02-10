using System;
using System.Text.Json.Serialization;
using MongoDB.Driver;

namespace Data.Repositories.Options;

public class MongoOptions
{
	private readonly Lazy<MongoUrl> _mongoUrl;

	public MongoOptions()
    {
		_mongoUrl = new Lazy<MongoUrl>(() => new MongoUrl(ConnectionString));
	}

	public string ConnectionString { get; set; } = null!;
	[JsonIgnore]
	public string DatabaseName => _mongoUrl.Value.DatabaseName;
}

