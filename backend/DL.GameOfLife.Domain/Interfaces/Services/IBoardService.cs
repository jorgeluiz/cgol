using System;
using DL.GameOfLife.Domain.Entities;

namespace DL.GameOfLife.Domain.Interfaces.Services;

public interface IBoardService
{
    Task<Board> CreateAsync(Board board);
    Task<Board> FindByIdAsync(string boardId);
    Task DeleteByIdAsync(string boardId);
}
