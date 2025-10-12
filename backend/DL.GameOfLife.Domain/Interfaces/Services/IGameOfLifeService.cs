using DL.GameOfLife.Models;
using DL.GameOfLife.Domain.Common;

namespace DL.GameOfLife.Domain.Interfaces.Services;

public interface IGameOfLifeService
{
    public Task<OperationResult<BoardModelResponse>> NewGame(BoardModelRequest boardModel);
    public Task<OperationResult<BoardModelResponse>> UpdateGame(UpdateBoardModelRequest boardModel);
    public Task<OperationResult<BoardModelResponse>> LoadGame(string boardId);
    public Task<OperationResult<BoardModelResponse>> NextState(string boardId);
    public Task<OperationResult<BoardModelResponse>> IncrementState(string boardId, int statesToIncrement);
    public Task<OperationResult<BoardModelResponse>> IncrementTillTheLimit(string boardId);
    public Task<OperationResult<long>> EndGame(string boardId);
}
