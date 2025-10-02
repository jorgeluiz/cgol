using DL.GameOfLife.Domain.Entities;
using DL.GameOfLife.Domain.Interfaces.Repositories;
using DL.GameOfLife.Domain.Interfaces.Services;

namespace DL.GameOfLife.Service;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _repository;

    public BoardService(IBoardRepository repository)
    {
        _repository = repository;
    }

    public async Task<Board> CreateAsync(Board board)
    {
        return await _repository.InsertAsync(board);
    }

    public async Task<Board> FindByIdAsync(string boardId)
    {
        return await _repository.FindByIdAsync(boardId);
    }
    public async Task DeleteByIdAsync(string boardId)
    {
        await _repository.RemoveByIdAsync(boardId);
    }
}
