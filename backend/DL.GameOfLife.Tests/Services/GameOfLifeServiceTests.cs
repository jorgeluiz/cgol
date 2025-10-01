using Moq;
using Microsoft.Extensions.Logging;
using DL.GameOfLife.Domain.Interfaces.Services;
using DL.GameOfLife.Service;
using DL.GameOfLife.Domain.Entities;

namespace DL.GameOfLife.Tests
{
    public class GameOfLifeServiceTests
    {
        private readonly Mock<ILogger<GameOfLifeService>> _mockLogger;
        private readonly Mock<IBoardService> _mockBoardService;
        private readonly GameOfLifeService _gameOfLifeService;


        public GameOfLifeServiceTests()
        {
            _mockLogger = new Mock<ILogger<GameOfLifeService>>();
            _mockBoardService = new Mock<IBoardService>();
            _gameOfLifeService = new GameOfLifeService(_mockLogger.Object, _mockBoardService.Object);
        }

        [Fact(DisplayName = @"Given a valid game to calculate, 
            when a board has no alive cells, 
            then should return same board")]
        public async Task Calculate_ShouldReturnSameBoard_WhenNoCellsAreAlive()
        {
            // Arrange
            var board = new Board
            {
                Cells = new List<BoardCell>
                {
                    new BoardCell { ColumnNumber = 0, RowNumber = 0, IsAlive = false },
                    new BoardCell { ColumnNumber = 0, RowNumber = 1, IsAlive = false },
                    new BoardCell { ColumnNumber = 1, RowNumber = 0, IsAlive = false },
                    new BoardCell { ColumnNumber = 1, RowNumber = 1, IsAlive = false }
                }
            };

            // Act
            var result = await _gameOfLifeService.Calculate(board);

            // Assert
            Assert.Same(board, result);
        }

        [Fact(DisplayName = @"Given a board with just one alive cell, 
            when rule number one is applied, 
            then that cell should die")]
        public async Task Calculate_RuleNumberOne_LiveCellWithFewerThanTwoLiveNeighboursDies()
        {
            // Arrange: Alive cell (1,1) with only one alive cell (0,0)
            var board = new Board
            {
                Cells = new List<BoardCell>
                {
                    new BoardCell { ColumnNumber = 0, RowNumber = 0, IsAlive = true },
                    new BoardCell { ColumnNumber = 1, RowNumber = 1, IsAlive = true }
                }
            };

            // Act
            var newBoard = await _gameOfLifeService.Calculate(board);

            // Assert
            var cellUnderTest = newBoard.Cells.First(c => c.ColumnNumber == 1 && c.RowNumber == 1);
            Assert.False(cellUnderTest.IsAlive);
        }

        [Fact(DisplayName = @"Given a board with a live cell and two living neighbours, 
            when rule number two is applied, 
            then that cell should be kept alive")]
        public async Task Calculate_RuleNumberTwo_LiveCellWithTwoLiveNeighboursLives()
        {
            // Arrange: Alive cell (1,1) with two neighbours alive (0,0) e (0,1)
            var board = new Board
            {
                Cells = new List<BoardCell>
                {
                    new BoardCell { ColumnNumber = 0, RowNumber = 0, IsAlive = true },
                    new BoardCell { ColumnNumber = 0, RowNumber = 1, IsAlive = true },
                    new BoardCell { ColumnNumber = 1, RowNumber = 1, IsAlive = true }
                }
            };

            // Act
            var newBoard = await _gameOfLifeService.Calculate(board);

            // Assert
            var cellUnderTest = newBoard.Cells.First(c => c.ColumnNumber == 1 && c.RowNumber == 1);
            Assert.True(cellUnderTest.IsAlive);
        }

        [Fact(DisplayName = @"Given a board with a live cell and three living neighbours, 
            when rule number two is applied, 
            then that cell should be kept alive")]
        public async Task Calculate_RuleNumberTwo_LiveCellWithThreeLiveNeighboursLives()
        {
            // Arrange: Alive cell (1,1) with three alive neighbours
            var board = new Board
            {
                Cells = new List<BoardCell>
                {
                    new BoardCell { ColumnNumber = 0, RowNumber = 0, IsAlive = true },
                    new BoardCell { ColumnNumber = 0, RowNumber = 1, IsAlive = true },
                    new BoardCell { ColumnNumber = 0, RowNumber = 2, IsAlive = true },
                    new BoardCell { ColumnNumber = 1, RowNumber = 1, IsAlive = true }
                }
            };

            // Act
            var newBoard = await _gameOfLifeService.Calculate(board);

            // Assert
            var cellUnderTest = newBoard.Cells.First(c => c.ColumnNumber == 1 && c.RowNumber == 1);
            Assert.True(cellUnderTest.IsAlive);
        }


        [Fact(DisplayName = @"Given a board with a live cell and four living neighbours, 
            when rule number three is applied, 
            then that cell should die")]
        public async Task Calculate_RuleNumberThree_LiveCellWithMoreThanThreeLiveNeighboursDies()
        {
            // Arrange: Alive cell (1,1) with four alive neighbours
            var board = new Board
            {
                Cells = new List<BoardCell>
                {
                    new BoardCell { ColumnNumber = 0, RowNumber = 0, IsAlive = true },
                    new BoardCell { ColumnNumber = 0, RowNumber = 1, IsAlive = true },
                    new BoardCell { ColumnNumber = 0, RowNumber = 2, IsAlive = true },
                    new BoardCell { ColumnNumber = 1, RowNumber = 0, IsAlive = true },
                    new BoardCell { ColumnNumber = 1, RowNumber = 1, IsAlive = true }
                }
            };

            // Act
            var newBoard = await _gameOfLifeService.Calculate(board);

            // Assert
            var cellUnderTest = newBoard.Cells.First(c => c.ColumnNumber == 1 && c.RowNumber == 1);
            Assert.False(cellUnderTest.IsAlive);
        }

        [Fact(DisplayName = @"Given a board with a dead cell and three living neighbours, 
            when rule number four is applied, 
            then that cell should become alive")]
        public async Task Calculate_RuleNumberFour_DeadCellWithExactlyThreeLiveNeighboursBecomesAlive()
        {
            // Arrange: Dead cell (1,1) with three alive neighbours
            var board = new Board
            {
                Cells = new List<BoardCell>
                {
                    new BoardCell { ColumnNumber = 0, RowNumber = 1, IsAlive = true },
                    new BoardCell { ColumnNumber = 1, RowNumber = 0, IsAlive = false }, // dead cell in the middle
                    new BoardCell { ColumnNumber = 1, RowNumber = 1, IsAlive = true },
                    new BoardCell { ColumnNumber = 2, RowNumber = 1, IsAlive = true }
                }
            };

            // Act
            var newBoard = await _gameOfLifeService.Calculate(board);

            // Assert
            var cellUnderTest = newBoard.Cells.First(c => c.ColumnNumber == 1 && c.RowNumber == 0);
            var centerCell = newBoard.Cells.First(c => c.ColumnNumber == 1 && c.RowNumber == 1);
            Assert.True(centerCell.IsAlive);
        }

        [Fact(DisplayName = @"Given a board with a dead cell and three living neighbours, 
            when rule number four is applied, 
            then that cell should become alive")]
        public async Task Calculate_BlinkerPattern_ShouldOscillateCorrectly()
        {
            // Arrange: Blinker pattern horizontaly
            var board = new Board
            {
                Cells = new List<BoardCell>
                {
                    new BoardCell { ColumnNumber = 0, RowNumber = 1, IsAlive = true },
                    new BoardCell { ColumnNumber = 1, RowNumber = 1, IsAlive = true },
                    new BoardCell { ColumnNumber = 2, RowNumber = 1, IsAlive = true },
                    // Adding dead cells around it
                    new BoardCell { ColumnNumber = 1, RowNumber = 0, IsAlive = false },
                    new BoardCell { ColumnNumber = 1, RowNumber = 2, IsAlive = false }
                }
            };

            // Act
            var newBoard = await _gameOfLifeService.Calculate(board);

            // Assert: Should be transformed on a vertical pattern
            Assert.False(newBoard.Cells.First(c => c.ColumnNumber == 0 && c.RowNumber == 1).IsAlive);
            Assert.True(newBoard.Cells.First(c => c.ColumnNumber == 1 && c.RowNumber == 0).IsAlive);
            Assert.True(newBoard.Cells.First(c => c.ColumnNumber == 1 && c.RowNumber == 1).IsAlive);
            Assert.True(newBoard.Cells.First(c => c.ColumnNumber == 1 && c.RowNumber == 2).IsAlive);
            Assert.False(newBoard.Cells.First(c => c.ColumnNumber == 2 && c.RowNumber == 1).IsAlive);
        }
    }
}

