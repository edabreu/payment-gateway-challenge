using System;
using Data.Repositories.Options;
using Data.Repositories.Dbos.Extensions;
using Domain.Abstractions.Repositories;
using Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.Repositories;

public class PaymentsRepository : BaseRepository<Dbos.Payment>, IPaymentsRepository
{
	public PaymentsRepository(IMongoClient mongoClient, IOptions<MongoOptions> mongoOptions)
        : base(mongoClient, mongoOptions)
	{
	}

    protected override string CollectionName => "payments";

    public async Task<Payment> GetAsync(string id)
    {
        var dboPayment = await this.GetByIdAsync(id);

        return dboPayment.ToModel();
    }

    public async Task<Payment> InsertAsync(Payment payment)
    {
        var dboPayment = payment.ToDbo();
        await this.InsertOneAsync(dboPayment);
        return dboPayment.ToModel();
    }
}

