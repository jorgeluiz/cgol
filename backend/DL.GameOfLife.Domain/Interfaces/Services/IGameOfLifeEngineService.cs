using DL.GameOfLife.Domain.Entities;

namespace DL.GameOfLife.Domain.Interfaces.Services;

public interface IGameOfLifeEngineService
{
    public Task<Board> Calculate(Board currentState);
}
