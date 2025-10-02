using DL.GameOfLife.Data.Registers;
using DL.GameOfLife.Domain.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DL.GameOfLife.Data.Context;

public class GameOfLifeContext : IGameOfLifeContext
{
    private readonly MongoClient _mongoClient;
    private readonly DatabaseOptions _options;

    public GameOfLifeContext(IOptions<DatabaseOptions> options)
    {
        _options = options.Value;
        _mongoClient = new MongoClient(_options.ConnectionString);
    }

    public IMongoDatabase GetDatabase()
    {
        return _mongoClient.GetDatabase(_options.DatabaseName);
    }
}
