using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Repositories.Dbos;

public interface IDocument
{
	[BsonId]
	[BsonRepresentation(BsonType.String)]
	ObjectId Id { get; set; }
}
