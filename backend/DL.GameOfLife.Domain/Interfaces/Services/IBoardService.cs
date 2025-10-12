using System;
using DL.GameOfLife.Domain.Entities;

namespace DL.GameOfLife.Domain.Interfaces.Services;

public interface IBoardService
{
    Task<Board> CreateAsync(Board board);
    Task<Board> UpdateCellsAsync(Board board);
    Task<Board> FindByIdAsync(string boardId);
    Task<long> DeleteByIdAsync(string boardId);
}
