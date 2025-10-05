using DL.GameOfLife.Domain.Entities;

namespace DL.GameOfLife.Domain.Interfaces.Repositories;

public interface IBoardRepository
{
    Task<Board> InsertAsync(Board board);
    Task<Board> FindByIdAsync(string boardId);
    Task<long> RemoveByIdAsync(string boardId);
}
