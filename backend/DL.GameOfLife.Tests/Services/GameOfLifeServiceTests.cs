using Moq;
using Microsoft.Extensions.Logging;
using DL.GameOfLife.Domain.Interfaces.Services;
using DL.GameOfLife.Service;
using Microsoft.Extensions.Options;
using DL.GameOfLife.Domain.Options;
using AutoMapper;

namespace DL.GameOfLife.Tests
{
    public class GameOfLifeServiceTests
    {
        private readonly Mock<ILogger<GameOfLifeService>> _mockLogger;
        private readonly Mock<IBoardService> _mockBoardService;
        private readonly IGameOfLifeEngineService _gameEngineService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GameOfLifeService _gameOfLifeService;
        private readonly Mock<IOptions<GameOfLifeOptions>> _mockOptions;

        public GameOfLifeServiceTests()
        {
            _mockLogger = new Mock<ILogger<GameOfLifeService>>();
            _mockBoardService = new Mock<IBoardService>();
            _mockMapper = new Mock<IMapper>();
            _mockOptions = new Mock<IOptions<GameOfLifeOptions>>();

            var gameOfLifeOptions = new GameOfLifeOptions
            {
                ColumnStartOffset = -1,
                ColumnEndOffset = 1,
                RowStartOffset = -1,
                RowEndOffset = 1,
            };

            _gameEngineService = new GameOfLifeEngineService(Options.Create(gameOfLifeOptions));

            _mockOptions.Setup(x => x.Value).Returns(gameOfLifeOptions);

            _gameOfLifeService = new GameOfLifeService(_mockLogger.Object, _mockBoardService.Object, _gameEngineService, _mockOptions.Object, _mockMapper.Object);
        }

    }
}

