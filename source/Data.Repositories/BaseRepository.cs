using System.Linq.Expressions;
using Data.Repositories.Dbos;
using Data.Repositories.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.Repositories;

public abstract class BaseRepository<T>
    where T : IDocument
{
    private readonly IMongoClient _mongoClient;

    public BaseRepository(IMongoClient mongoClient, IOptions<MongoOptions> mongoOptions)
    {
        _mongoClient = mongoClient;
        Collection = mongoClient.GetDatabase(mongoOptions.Value.DatabaseName)
            .GetCollection<T>(CollectionName);
    }

    protected abstract string CollectionName { get; }

    protected IMongoCollection<T> Collection { get; }

    protected async Task<T> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq(document => document.Id, new ObjectId(id));
        return await this.Collection.Find(filter).FirstOrDefaultAsync();
    }

    protected Task InsertOneAsync(T document)
    {
        return this.Collection.InsertOneAsync(document);
    }
}