using System;
using DL.GameOfLife.Domain.Entities;

namespace DL.GameOfLife.Domain.Interfaces.Services;

public interface IBoardService
{
    public Board Create(Board board);
    public Board FindById(string boardId);
}
