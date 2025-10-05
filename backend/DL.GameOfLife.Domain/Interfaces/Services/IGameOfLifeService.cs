using DL.GameOfLife.Models;
using DL.GameOfLife.Domain.Common;
using DL.GameOfLife.Domain.Entities;

namespace DL.GameOfLife.Domain.Interfaces.Services;

public interface IGameOfLifeService
{
    public Task<OperationResult<BoardModel>> NewGame(BoardModel boardModel);
    public Task<OperationResult<BoardModel>> LoadGame(string boardId);
    public Task<OperationResult<long>> EndGame(string boardId);
    public Task<Board> Calculate(Board currentState);
}
