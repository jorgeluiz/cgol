using MongoDB.Driver;

namespace DL.GameOfLife.Data.Context;

public interface IGameOfLifeContext
{
    IMongoDatabase GetDatabase();
}
