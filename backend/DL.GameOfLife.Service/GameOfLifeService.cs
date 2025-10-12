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

    #region Basic actions
    /// <summary>
    /// Create a new board and generates an boardId
    /// </summary>
    /// <param name="boardModel">The object containing the new board</param>
    /// <returns>The newly created board</returns>
    public async Task<OperationResult<BoardModelResponse>> NewGame(BoardModelRequest boardModel)
    {
        var board = _mapper.Map<Board>(boardModel);

        var created = await _boardService.CreateAsync(board);

        var result = _mapper.Map<BoardModelResponse>(created);

        return OperationResult<BoardModelResponse>.Ok(result);

    }
    /// <summary>
    /// Update an existing board
    /// </summary>
    /// <param name="boardModel">The object containing an existing board</param>
    /// <returns>The updated board</returns>
    public async Task<OperationResult<BoardModelResponse>> UpdateGame(UpdateBoardModelRequest boardModel)
    {
        var board = _mapper.Map<Board>(boardModel);

        var created = await _boardService.UpdateCellsAsync(board);

        var result = _mapper.Map<BoardModelResponse>(created);
        
        return OperationResult<BoardModelResponse>.Ok(result);
    }

    /// <summary>
    /// Load a board an based on a boardId
    /// </summary>
    /// <param name="boardId">The unique id of a board</param>
    /// <returns>The stored board</returns>
    public async Task<OperationResult<BoardModelResponse>> LoadGame(string boardId)
    {
        var board = await _boardService.FindByIdAsync(boardId);

        if (board != null)
        {
            var result = _mapper.Map<BoardModelResponse>(board);
            return OperationResult<BoardModelResponse>.Ok(result);
        }

        return OperationResult<BoardModelResponse>.NotFound(ErrorCodes.ERR_0002.NewResultError());
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
    #endregion


    #region Navigation actions
    /// <summary>
    /// Move the board to the next state
    /// </summary>
    /// <param name="boardId">The unique id of a board</param>
    /// <returns>The new board</returns>
    public async Task<OperationResult<BoardModelResponse>> NextState(string boardId)
    {

        //Load the current state
        var board = await _boardService.FindByIdAsync(boardId);

        //Increase one state if the board exist
        if (board != null)
        {
            var newState = await IncreaseBoardStateBy(board, 1, false);
            //Map the result
            var result = _mapper.Map<BoardModelResponse>(newState);

            return OperationResult<BoardModelResponse>.Ok(result);
        }

        return OperationResult<BoardModelResponse>.NotFound(ErrorCodes.ERR_0002.NewResultError());
    }

    /// <summary>
    /// Move the board to the defined state
    /// </summary>
    /// <param name="boardId">The unique id of a board</param>
    /// <returns>The new board</returns>
    public async Task<OperationResult<BoardModelResponse>> IncrementState(string boardId, int statesToIncrement)
    {

        //Load the current state
        var board = await _boardService.FindByIdAsync(boardId);

        //Increase one state if the board exist
        if (board != null)
        {
            var newState = await IncreaseBoardStateBy(board, statesToIncrement);

            //Map the result
            var result = _mapper.Map<BoardModelResponse>(newState);

            if (statesToIncrement > _options.StatesIncrementLimit)
            {
                result.Errors.Add(ErrorCodes.ERR_0003.NewErrorModel());
            }

            return OperationResult<BoardModelResponse>.Ok(result);
        }

        return OperationResult<BoardModelResponse>.NotFound(ErrorCodes.ERR_0002.NewResultError());
    }

    /// <summary>
    /// Move the board to the defined state
    /// </summary>
    /// <param name="boardId">The unique id of a board</param>
    /// <returns>The new board</returns>
    public async Task<OperationResult<BoardModelResponse>> IncrementTillTheLimit(string boardId)
    {

        //Load the current state
        var board = await _boardService.FindByIdAsync(boardId);

        //Increase one state if the board exist
        if (board != null)
        {
            var newState = await IncreaseBoardStateBy(board, _options.StatesIncrementLimit);

            //Map the result
            var result = _mapper.Map<BoardModelResponse>(newState);
            result.Errors.Add(ErrorCodes.ERR_0003.NewErrorModel());

            return OperationResult<BoardModelResponse>.Ok(result);
        }

        return OperationResult<BoardModelResponse>.NotFound(ErrorCodes.ERR_0002.NewResultError());
    }

    /// <summary>
    /// Move the board to the next state
    /// </summary>
    /// <param name="boardId">The unique id of a board</param>
    /// <param name="statesMove">The desired increment number</param>
    /// <param name="verifyLimit">Whether it is necessary to check for the application state increment limit</param>
    /// <returns>The last valid board</returns>
    private async Task<Board> IncreaseBoardStateBy(Board board, int statesMove, bool verifyLimit = true)
    {
        Board output = new();
        int statesLimit = statesMove;

        //Avoid infinite calculations
        if (verifyLimit && statesMove > _options.StatesIncrementLimit)
        {
            statesLimit = _options.StatesIncrementLimit;
        }

        for (int i = 0; i < statesLimit; i++)
        {
            //Get the next Generation
            var newState = await _gameEngineService.Calculate(board);

            //Set the parentId to preserve the history
            newState.ParentId = board.Id;

            //Store the new state
            var newStateSaved = await _boardService.CreateAsync(newState);

            output = newStateSaved;
        }

        return output;
    }

    #endregion
}
