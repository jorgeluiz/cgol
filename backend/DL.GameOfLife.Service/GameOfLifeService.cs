using System;
using System.Collections.Concurrent;
using DL.GameOfLife.Domain.Entities;
using DL.GameOfLife.Domain.Interfaces.Services;
using DL.GameOfLife.Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DL.GameOfLife.Service;

public class GameOfLifeService : IGameOfLifeService
{
    private readonly ILogger<GameOfLifeService> _logger;
    private readonly IBoardService _boardService;
    private readonly GameOfLifeOptions _options;
    private readonly int _columnStartOffset;
    private readonly int _columnEndOffset;
    private readonly int _rowEndOffset;
    private readonly int _rowStartOffset;
    public GameOfLifeService(ILogger<GameOfLifeService> logger, IBoardService boardService, IOptions<GameOfLifeOptions> options)
    {
        _logger = logger;
        _boardService = boardService;
        _options = options.Value;

        _columnStartOffset = _options.ColumnStartOffset;
        _columnEndOffset = _options.ColumnEndOffset;
        _rowStartOffset = _options.RowStartOffset;
        _rowEndOffset = _options.RowEndOffset;
    }

    /// <summary>
    /// Create a new board an generates an boardId
    /// </summary>
    /// <param name="board">The object containing the new board</param>
    /// <returns>The newly created board</returns>
    public async Task<Board> NewGame(Board board)
    {
        return await _boardService.CreateAsync(board);
    }

    /// <summary>
    /// Load a board an based on a boardId
    /// </summary>
    /// <param name="boardId">The unique id of a board</param>
    /// <returns>The stored board</returns>
    public async Task<Board> LoadGame(string boardId)
    {
        return await _boardService.FindByIdAsync(boardId);
    }

    /// <summary>
    /// The main method that calculates the board states 
    /// </summary>
    /// <param name="currentState">The current state of the board</param>
    /// <returns></returns>
    public async Task<Board> Calculate(Board currentState)
    {
        //Return the same board if there is no live cells
        if (!currentState.Cells.Any(x => x.IsAlive))
        {
            return currentState;
        }

        //Go trought the board and apply the game logic
        return await CheckBoardCells(currentState);
    }

    /// <summary>
    /// Verify board cells and check if they 
    /// will be alive or not in the next generation
    /// </summary>
    /// <param name="currentState"> The current state of the game</param>
    /// <returns>The new state of the game</returns>
    private async Task<Board> CheckBoardCells(Board currentState)
    {
        Board newBoard = new();

        //Convert cells to a dictionary for better performance during search
        Dictionary<(int, int), BoardCell> allCells = currentState.Cells.ToDictionary(x => (x.ColumnNumber, x.RowNumber));

        ConcurrentBag<BoardCell> newBoardCells = new();

        ParallelOptions parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 3
        };

        await Parallel.ForEachAsync(currentState.Cells, parallelOptions, async (cell, cancellationToken) =>
        {
            await Task.Run(() =>
            {
                var isAlive = WillBeAlive(cell, allCells);
                newBoardCells.Add(new BoardCell { IsAlive = isAlive, ColumnNumber = cell.ColumnNumber, RowNumber = cell.RowNumber });
            });
        });

        newBoard.Cells = newBoardCells.ToList();

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
