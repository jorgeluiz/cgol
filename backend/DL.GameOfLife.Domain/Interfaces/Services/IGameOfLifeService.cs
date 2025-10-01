using System;
using DL.GameOfLife.Domain.Entities;

namespace DL.GameOfLife.Domain.Interfaces.Services;

public interface IGameOfLifeService
{
    public Board Calculate(Board currentState);
}
