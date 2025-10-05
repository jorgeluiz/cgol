using DL.GameOfLife.Models;
using DL.GameOfLife.Domain.Common;

namespace DL.GameOfLife.Domain.Interfaces.Services;

public interface IGameOfLifeService
{
    public Task<OperationResult<BoardModel>> NewGame(BoardModel boardModel);
    public Task<OperationResult<BoardModel>> LoadGame(string boardId);
    public Task<OperationResult<long>> EndGame(string boardId);
}
