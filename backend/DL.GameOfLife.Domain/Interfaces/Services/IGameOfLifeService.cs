using System;
using DL.GameOfLife.Domain.Entities;

namespace DL.GameOfLife.Domain.Interfaces.Services;

public interface IGameOfLifeService
{
    public Task<Board> Calculate(Board currentState);
}
