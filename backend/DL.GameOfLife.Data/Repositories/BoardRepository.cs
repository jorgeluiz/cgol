using DL.GameOfLife.Data.Context;
using DL.GameOfLife.Domain.Entities;
using DL.GameOfLife.Domain.Interfaces.Repositories;
using MongoDB.Driver;

namespace DL.GameOfLife.Data.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly IMongoCollection<Board> _collection;
    public BoardRepository(IGameOfLifeContext dbContext)
    {
        _collection = dbContext.GetDatabase().GetCollection<Board>("Boards");
    }

    public async Task<Board> InsertAsync(Board board)
    {
        //Create an ID for the new board
        board.Id = Guid.NewGuid().ToString();

        await _collection.InsertOneAsync(board);

        return board;

    }

    public async Task<Board> UpdateCellsAsync(Board board)
    {
        var filter = Builders<Board>.Filter.Eq(d => d.Id, board.Id);
        var update = Builders<Board>.Update.Set(d => d.Cells, board.Cells);

        var options = new FindOneAndUpdateOptions<Board>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        var result = await _collection.FindOneAndUpdateAsync(filter, update, options);

        return board;

    }
    public async Task<Board> FindByIdAsync(string boardId)
    {
        var filters = Builders<Board>.Filter.Eq(x => x.Id, boardId);

        return await _collection.Find(filters).Limit(1).FirstOrDefaultAsync();
    }

    public async Task<long> RemoveByIdAsync(string boardId)
    {
        var filters = Builders<Board>.Filter.Eq(x => x.Id, boardId);

        var result = await _collection.DeleteOneAsync(filters);

        return result.DeletedCount;

    }
}
