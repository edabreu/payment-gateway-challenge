using System;
using MongoDB.Bson;

namespace Data.Repositories.Dbos;

public abstract class Document : IDocument
{
    public ObjectId Id { get; set; }
}