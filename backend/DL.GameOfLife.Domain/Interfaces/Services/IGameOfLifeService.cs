using System;
using DL.GameOfLife.Domain.Entities;

namespace DL.GameOfLife.Domain.Interfaces.Services;

public interface IGameOfLifeService
{
    public Task<Board> NewGame(Board board);
    public Task<Board> LoadGame(string boardId);
    public Task<long> EndGame(string boardId);
    public Task<Board> Calculate(Board currentState);
}
