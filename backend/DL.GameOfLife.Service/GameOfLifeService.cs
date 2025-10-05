using DL.GameOfLife.Models;
using DL.GameOfLife.Domain.Common;
using DL.GameOfLife.Domain.Entities;
using DL.GameOfLife.Domain.Enums;
using DL.GameOfLife.Domain.Extensions;
using DL.GameOfLife.Domain.Interfaces.Services;
using DL.GameOfLife.Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;

namespace DL.GameOfLife.Service;

public class GameOfLifeService : IGameOfLifeService
{
    private readonly ILogger<GameOfLifeService> _logger;
    private readonly IMapper _mapper;
    private readonly IBoardService _boardService;
    private readonly IGameOfLifeEngineService _gameEngineService;
    private readonly GameOfLifeOptions _options;
    public GameOfLifeService(ILogger<GameOfLifeService> logger, IBoardService boardService, IGameOfLifeEngineService gameEngineService, IOptions<GameOfLifeOptions> options, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        _boardService = boardService;
        _options = options.Value;
        _gameEngineService = gameEngineService;

    }

    /// <summary>
    /// Create a new board an generates an boardId
    /// </summary>
    /// <param name="boardModel">The object containing the new board</param>
    /// <returns>The newly created board</returns>
    public async Task<OperationResult<BoardModel>> NewGame(BoardModel boardModel)
    {
        var board = _mapper.Map<Board>(boardModel);

        var created = await _boardService.CreateAsync(board);

        var result = _mapper.Map<BoardModel>(created);

        return OperationResult<BoardModel>.Ok(result);

    }

    /// <summary>
    /// Load a board an based on a boardId
    /// </summary>
    /// <param name="boardId">The unique id of a board</param>
    /// <returns>The stored board</returns>
    public async Task<OperationResult<BoardModel>> LoadGame(string boardId)
    {
        var board = await _boardService.FindByIdAsync(boardId);

        if (board != null)
        {
            var result = _mapper.Map<BoardModel>(board);
            return OperationResult<BoardModel>.Ok(result);
        }

        return OperationResult<BoardModel>.NotFound(ErrorCodes.ERR_0002.NewResultError());
    }

    /// <summary>
    /// End a game
    /// </summary>
    /// <param name="boardId">The unique id of a board</param>
    /// <returns>Delete all data related to a game</returns>
    public async Task<OperationResult<long>> EndGame(string boardId)
    {
        var deleteCount = await _boardService.DeleteByIdAsync(boardId);

        if (deleteCount <= 0)
        {
            return OperationResult<long>.Error(ErrorCodes.ERR_0001.NewResultError());
        }

        return OperationResult<long>.Ok(deleteCount);
    }
}
