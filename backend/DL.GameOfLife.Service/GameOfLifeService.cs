using System;
using DL.GameOfLife.Domain.Entities;
using DL.GameOfLife.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DL.GameOfLife.Service;

public class GameOfLifeService : IGameOfLifeService
{
    private readonly ILogger<GameOfLifeService> _logger;
    private readonly IBoardService _boardService;
    private readonly int _columnStartOffset;
    private readonly int _columnEndOffset;
    private readonly int _rowEndOffset;
    private readonly int _rowStartOffset;
    public GameOfLifeService(ILogger<GameOfLifeService> logger, IBoardService boardService)
    {
        _logger = logger;
        _boardService = boardService;
        _columnStartOffset = -1;
        _columnEndOffset = 1;
        _rowStartOffset = -1;
        _rowEndOffset = 1;
    }

    /// <summary>
    /// The main method that calculates the board states 
    /// </summary>
    /// <param name="currentState">The current state of the board</param>
    /// <returns></returns>
    public Board Calculate(Board currentState)
    {
        //Return the same board if there is no live cells
        if (!currentState.Cells.Any(x => x.IsAlive))
        {
            return currentState;
        }

        //Go trought the board and apply the game logic
        return CheckBoardCells(currentState);
    }

    /// <summary>
    /// Verify board cells and check if they 
    /// will be alive or not in the next generation
    /// </summary>
    /// <param name="currentState"> The current state of the game</param>
    /// <returns>The new state of the game</returns>
    private Board CheckBoardCells(Board currentState)
    {
        Board newBoard = new();

        //Convert cells to a dictionary for better performance during search
        Dictionary<(int, int), BoardCell> allCells = currentState.Cells.ToDictionary(x => (x.ColumnNumber, x.RowNumber));


        //Iterate trought cells and check his neighbours
        foreach (var cell in currentState.Cells)
        {
            var isAlive = WillBeAlive(cell, allCells);
            newBoard.Cells.Add(new BoardCell { IsAlive = isAlive, ColumnNumber = cell.ColumnNumber, RowNumber = cell.RowNumber });
        }

        return newBoard;
    }

    /// <summary>
    ///  Apply the game rules to check if the cell will be alive
    /// </summary>
    /// <param name="cell">The cell to be checked</param>
    /// <param name="allCells">A dictionary containing all the other cells to search for the neighbours</param>
    /// <returns>True or false rather the cell will be alive or not</returns>
    private bool WillBeAlive(BoardCell cell, Dictionary<(int, int), BoardCell> allCells)
    {
        var myNeighbours = FindNeighbours(cell.ColumnNumber, cell.RowNumber, allCells);
        var totalAliveNeighbours = myNeighbours.Count(x => x.IsAlive);

        //Rule #1 - Any live cell with fewer than two live neighbours dies, as if by underpopulation.
        if (cell.IsAlive && totalAliveNeighbours < 2)
        {
            return false;
        }

        //Rule #2 - Any live cell with two or three live neighbours lives on to the next generation.
        if (cell.IsAlive && (totalAliveNeighbours == 2 || totalAliveNeighbours == 3))
        {
            return true;
        }

        //Rule #3 - Any live cell with more than three live neighbours dies, as if by overpopulation.
        if (cell.IsAlive && totalAliveNeighbours > 3)
        {
            return false;
        }

        //Rule #4 - Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
         if (!cell.IsAlive && totalAliveNeighbours == 3)
        {
            return true;
        }

        //Everything else counts as a dead cell
        return false;
    }
    /// <summary>
    /// Retreive all the neighbours of the current cell
    /// </summary>
    /// <param name="columnNumber">The current cell column number</param>
    /// <param name="rowNumber">The current cell row number/param>
    /// <param name="allCells">A dictionary with all cells of the board</param>
    /// <returns>A list with all cell neighbours</returns>
    private List<BoardCell> FindNeighbours(int columnNumber, int rowNumber, Dictionary<(int, int), BoardCell> allCells)
    {
        List<BoardCell> neighbours = new();

        var neighboursKeys = GenerateNeighboursKeys(columnNumber, rowNumber);

        foreach (var neighbourKey in neighboursKeys)
        {
            if (allCells.TryGetValue(neighbourKey, out var neighbour))
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }
    /// <summary>
    ///  Create a list with the neighbour keys to be searched latter
    /// </summary>
    /// <param name="cellColumn">The column number of the current cell</param>
    /// <param name="cellRow">The row number of the current cell</param>
    /// <returns>A list containing all the possibile keys bases on the offset configurations</returns>
    private List<(int, int)> GenerateNeighboursKeys(int cellColumn, int cellRow)
    {
        List<(int, int)> neighbourKeys = new();

        for (var columnNumber = _columnStartOffset; columnNumber <= _columnEndOffset; columnNumber++)
        {
            for (var rowNumber = _rowStartOffset; rowNumber <= _rowEndOffset; rowNumber++)
            {
                //Skip the cell who called this function
                if (columnNumber == 0 && rowNumber == 0)
                {
                    continue;
                }
                neighbourKeys.Add((cellColumn + columnNumber, cellRow + rowNumber));
            }
        }

        return neighbourKeys;
    }
}
