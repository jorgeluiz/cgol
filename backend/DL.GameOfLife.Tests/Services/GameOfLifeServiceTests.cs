using Moq;
using Microsoft.Extensions.Logging;
using DL.GameOfLife.Domain.Interfaces.Services;
using DL.GameOfLife.Service;
using Microsoft.Extensions.Options;
using DL.GameOfLife.Domain.Options;
using AutoMapper;
using DL.GameOfLife.Domain.Entities;
using DL.GameOfLife.Models;
using DL.GameOfLife.Domain.Enums;
using DL.GameOfLife.Domain.Extensions;

namespace DL.GameOfLife.Tests
{
    public class GameOfLifeServiceTests
    {
        private readonly Mock<ILogger<GameOfLifeService>> _mockLogger;
        private readonly Mock<IBoardService> _mockBoardService;
        private readonly Mock<IGameOfLifeEngineService> _mockGameEngineService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GameOfLifeService _gameOfLifeService;
        private readonly GameOfLifeOptions _options;
        private readonly Mock<IOptions<GameOfLifeOptions>> _mockOptions;

        public GameOfLifeServiceTests()
        {
            _mockLogger = new Mock<ILogger<GameOfLifeService>>();
            _mockBoardService = new Mock<IBoardService>();
            _mockMapper = new Mock<IMapper>();
            _mockOptions = new Mock<IOptions<GameOfLifeOptions>>();
            _mockGameEngineService = new Mock<IGameOfLifeEngineService>();
            _options = new GameOfLifeOptions { StatesIncrementLimit = 10 };

            _mockOptions.Setup(x => x.Value).Returns(_options);

            _gameOfLifeService = new GameOfLifeService(_mockLogger.Object, _mockBoardService.Object, _mockGameEngineService.Object, _mockOptions.Object, _mockMapper.Object);
        }

        [Fact(DisplayName = @"Given a valid new board request, 
        when NewGame is called, 
        then it should create the board and return the mapped response")]
        public async Task NewGame_ShouldCreateAndReturnBoard_WhenCalled()
        {
            // Arrange
            var request = new BoardModelRequest();
            var boardEntity = new Board();
            var createdBoardEntity = new Board { Id = "board-x" };
            var expectedResponse = new BoardModelResponse { Id = "board-x" };

            _mockMapper.Setup(m => m.Map<Board>(request)).Returns(boardEntity);
            _mockBoardService.Setup(s => s.CreateAsync(boardEntity)).ReturnsAsync(createdBoardEntity);
            _mockMapper.Setup(m => m.Map<BoardModelResponse>(createdBoardEntity)).Returns(expectedResponse);

            // Act
            var result = await _gameOfLifeService.NewGame(request);

            // Assert 
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedResponse, result.Model);
            _mockBoardService.Verify(s => s.CreateAsync(It.IsAny<Board>()), Times.Once);
        }

        [Fact(DisplayName = @"Given a valid existing board request,
        when UpdateGame is called,
        then it should update the board and return the mapped response")]
        public async Task UpdateGame_ShouldUpdateAndReturnBoard_WhenCalled()
        {
            // Arrange
            var request = new UpdateBoardModelRequest{Id = "board-y"};
            var boardEntity = new Board();
            var updatedBoardEntity = new Board { Id = "board-y" };
            var expectedResponse = new BoardModelResponse { Id = "board-y" };

            _mockMapper.Setup(m => m.Map<Board>(request)).Returns(boardEntity);
            _mockBoardService.Setup(s => s.UpdateCellsAsync(boardEntity)).ReturnsAsync(updatedBoardEntity);
            _mockMapper.Setup(m => m.Map<BoardModelResponse>(updatedBoardEntity)).Returns(expectedResponse);

            // Act
            var result = await _gameOfLifeService.UpdateGame(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedResponse, result.Model);
            _mockBoardService.Verify(s => s.UpdateCellsAsync(It.IsAny<Board>()), Times.Once);
        }

        [Fact(DisplayName = @"Given an ID for an existing board,
        when LoadGame is called, 
        then it should find and return the board")]
        public async Task LoadGame_ShouldReturnBoard_WhenBoardExists()
        {
            // Arrange
            var boardId = "existing-board";
            var boardEntity = new Board { Id = boardId };
            var expectedResponse = new BoardModelResponse { Id = boardId };
            _mockBoardService.Setup(s => s.FindByIdAsync(boardId)).ReturnsAsync(boardEntity);
            _mockMapper.Setup(m => m.Map<BoardModelResponse>(boardEntity)).Returns(expectedResponse);

            // Act
            var result = await _gameOfLifeService.LoadGame(boardId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedResponse, result.Model);
        }

        [Fact(DisplayName = @"Given an ID for a non-existing board, 
        when LoadGame is called, 
        then it should return a NotFound result")]
        public async Task LoadGame_ShouldReturnNotFound_WhenBoardDoesNotExist()
        {
            // Arrange
            var boardId = "non-existing-board";
            _ = _mockBoardService.Setup(s => s.FindByIdAsync(boardId)).ReturnsAsync((Board?)null);

            // Act
            var result = await _gameOfLifeService.LoadGame(boardId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Model);
            Assert.Equal(ErrorCodes.ERR_0002.Code(), result.Errors.FirstOrDefault()?.Code);
        }

        [Fact(DisplayName = @"Given an ID for a board to be deleted, 
        when EndGame is called, 
        then it should return a success result")]
        public async Task EndGame_ShouldReturnSuccess_WhenBoardIsDeleted()
        {
            // Arrange
            var boardId = "board-to-delete";
            _mockBoardService.Setup(s => s.DeleteByIdAsync(boardId)).ReturnsAsync(1);

            // Act
            var result = await _gameOfLifeService.EndGame(boardId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Model);
        }

        [Fact(DisplayName = @"Given an ID for a non-existing board, 
        when EndGame is called, 
        then it should return an error result")]
        public async Task EndGame_ShouldReturnError_WhenBoardToDeleteNotFound()
        {
            // Arrange
            var boardId = "non-existing-board";
            _mockBoardService.Setup(s => s.DeleteByIdAsync(boardId)).ReturnsAsync(0);

            // Act
            var result = await _gameOfLifeService.EndGame(boardId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCodes.ERR_0001.Code(), result.Errors.FirstOrDefault().Code);
        }

        [Fact(DisplayName = @"Given an existing board ID, 
        when NextState is called, 
        then it should calculate and save the next generation")]
        public async Task NextState_ShouldCalculateAndSaveNextGeneration_WhenBoardExists()
        {
            // Arrange
            var boardId = "board-1";
            var currentBoard = new Board { Id = boardId };
            var nextState = new Board();
            var savedNextState = new Board { Id = "board-2", ParentId = boardId };
            var expectedResponse = new BoardModelResponse { Id = "board-2" };
            _mockBoardService.Setup(s => s.FindByIdAsync(boardId)).ReturnsAsync(currentBoard);
            _mockGameEngineService.Setup(e => e.Calculate(currentBoard)).ReturnsAsync(nextState);
            _mockBoardService.Setup(s => s.CreateAsync(nextState)).ReturnsAsync(savedNextState);
            _mockMapper.Setup(m => m.Map<BoardModelResponse>(savedNextState)).Returns(expectedResponse);

            // Act
            var result = await _gameOfLifeService.NextState(boardId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedResponse, result.Model);
            _mockGameEngineService.Verify(e => e.Calculate(It.IsAny<Board>()), Times.Once);
            _mockBoardService.Verify(s => s.CreateAsync(It.IsAny<Board>()), Times.Once);
        }

        [Fact(DisplayName = @"Given states to increment within the limit, 
        when IncrementState is called, 
        then it should evolve the board for the specified number of times")]
        public async Task IncrementState_ShouldLoopCorrectNumberOfTimes_WhenWithinLimit()
        {
            // Arrange
            var boardId = "board-1";
            var statesToIncrement = 3;
            var initialBoard = new Board { Id = boardId };
            _mockBoardService.Setup(s => s.FindByIdAsync(boardId)).ReturnsAsync(initialBoard);
            _mockGameEngineService.Setup(e => e.Calculate(It.IsAny<Board>())).ReturnsAsync(new Board());
            _mockBoardService.Setup(s => s.CreateAsync(It.IsAny<Board>())).ReturnsAsync(new Board());
            _mockMapper.Setup(m => m.Map<BoardModelResponse>(It.IsAny<Board>())).Returns(new BoardModelResponse());

            // Act
            var result = await _gameOfLifeService.IncrementState(boardId, statesToIncrement);

            // Assert
            Assert.True(result.IsSuccess);
            _mockGameEngineService.Verify(e => e.Calculate(It.IsAny<Board>()), Times.Exactly(statesToIncrement));
            _mockBoardService.Verify(s => s.CreateAsync(It.IsAny<Board>()), Times.Exactly(statesToIncrement));
            Assert.DoesNotContain(result.Model.Errors, e => e.Code == ErrorCodes.ERR_0003.Code());
        }

        [Fact(DisplayName = @"Given states to increment exceeding the limit, 
        when IncrementState is called, 
        then it should cap the evolution at the limit and add a warning")]
        public async Task IncrementState_ShouldCapAtLimitAndAddWarning_WhenStatesToIncrementExceedsLimit()
        {
            // Arrange
            var boardId = "board-1";
            var statesToIncrement = 15; // > limit of 10
            var initialBoard = new Board { Id = boardId };
            var finalResponse = new BoardModelResponse();
            _mockBoardService.Setup(s => s.FindByIdAsync(boardId)).ReturnsAsync(initialBoard);
            _mockGameEngineService.Setup(e => e.Calculate(It.IsAny<Board>())).ReturnsAsync(new Board());
            _mockBoardService.Setup(s => s.CreateAsync(It.IsAny<Board>())).ReturnsAsync(new Board());
            _mockMapper.Setup(m => m.Map<BoardModelResponse>(It.IsAny<Board>())).Returns(finalResponse);

            // Act
            var result = await _gameOfLifeService.IncrementState(boardId, statesToIncrement);

            // Assert
            Assert.True(result.IsSuccess);
            _mockGameEngineService.Verify(e => e.Calculate(It.IsAny<Board>()), Times.Exactly(_options.StatesIncrementLimit));
            Assert.Contains(result.Model.Errors, e => e.Code == ErrorCodes.ERR_0003.Code());
        }
    }
}

