using AutoMapper;
using DL.GameOfLife.Models;
using DL.GameOfLife.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using DL.GameOfLife.Api.ErrorHandling;
namespace DL.GameOfLife.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GameOfLifeController : ControllerBase
{

    private readonly ILogger<GameOfLifeController> _logger;
    private readonly IMapper _mapper;
    private readonly IGameOfLifeService _service;

    public GameOfLifeController(ILogger<GameOfLifeController> logger, IMapper mapper, IGameOfLifeService service)
    {
        _logger = logger;
        _mapper = mapper;
        _service = service;
    }

    /// <summary>
    /// Creates a board
    /// </summary>
    /// <remarks>
    /// Initializes the first state of the game and returns the board identifier.
    /// </remarks>
    /// <param name="newBoard">An object that represents the initial state of the game.</param>
    /// <returns>The newly created game.</returns>
    /// <response code="200">The game was successfully created.</response>
    /// <response code="400">The game provided for creation is invalid.</response>
    /// <response code="500">An internal error occurred.</response>
    [HttpPost]
    [ProducesResponseType(typeof(BoardModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] BoardModelRequest newBoard)
    {
        var result = await _service.NewGame(newBoard);

        if (result.IsSuccess)
        {
            return Ok(result.Model);
        }

        return BadRequest(result.ErrorResponse());

    }

    /// <summary>
    /// Gets the current state of a board
    /// </summary>
    /// <remarks>
    /// Retrieves the current state of the game based on its identifier.
    /// </remarks>
    /// <param name="boardId">The unique identifier of the board.</param>
    /// <returns>The game stored on the server.</returns>
    /// <response code="200">The game was successfully retrieved from the server.</response>
    /// <response code="400">The request to load the game is invalid.</response>
    /// <response code="500">An internal error occurred.</response>
    [HttpGet("{boardId}")]
    [ProducesResponseType(typeof(BoardModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(string boardId)
    {
        var result = await _service.LoadGame(boardId);

        if (result.IsSuccess)
        {
            return Ok(result.Model);
        }

        return BadRequest(result.ErrorResponse());
    }

    /// <summary>
    /// Advances to the next state
    /// </summary>
    /// <remarks>
    /// Moves the game to the next state based on the current board identifier.
    /// </remarks>
    /// <param name="boardId">The unique identifier of the board.</param>
    /// <returns>The new state of the game.</returns>
    /// <response code="200">The latest state of the board after calculation.</response>
    /// <response code="400">The request to calculate the next state is invalid.</response>
    /// <response code="500">An internal error occurred.</response>
    [HttpGet("next_state/{boardId}")]
    [ProducesResponseType(typeof(BoardModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> NextState(string boardId)
    {
        var result = await _service.NextState(boardId);

        if (result.IsSuccess)
        {
            return Ok(result.Model);
        }

        return BadRequest(result.ErrorResponse());
    }

    /// <summary>
    /// Increments the current game by a specified number of states
    /// </summary>
    /// <remarks>
    /// Advances the game to a future state based on the specified number of increments.
    /// </remarks>
    /// <param name="boardId">The unique identifier of the board.</param>
    /// <param name="statesToIncrement">The number of states to advance from the current board state.</param>
    /// <returns>
    /// The state of the game after processing the specified number of increments. 
    /// If the increments exceed the allowed limit, an error message will be returned along with the last valid state.
    /// </returns>
    /// <response code="200">The final state of the board after advancing the requested number of states.</response>
    /// <response code="400">The request to calculate the future state is invalid.</response>
    /// <response code="500">An internal error occurred.</response>
    [HttpGet("increment_state/{boardId}/{statesToIncrement}")]
    [ProducesResponseType(typeof(BoardModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> IncrementState(string boardId, int statesToIncrement)
    {
        var result = await _service.IncrementState(boardId, statesToIncrement);

        if (result.IsSuccess)
        {
            return Ok(result.Model);
        }

        return BadRequest(result.ErrorResponse());
    }

    /// <summary>
    /// Final move
    /// </summary>
    /// <remarks>
    /// Advances the game to its final state based on the current board.
    /// </remarks>
    /// <param name="boardId">The unique identifier of the board.</param>
    /// <returns>The final possible state of the game after processing up to the calculation limit.</returns>
    /// <response code="200">The last state of the board.</response>
    /// <response code="400">The request to calculate the final state is invalid.</response>
    /// <response code="500">An internal error occurred.</response>
    [HttpGet("final/{boardId}")]
    [ProducesResponseType(typeof(BoardModelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GoToFinal(string boardId)
    {
        var result = await _service.IncrementTillTheLimit(boardId);

        if (result.IsSuccess)
        {
            return Ok(result.Model);
        }

        return BadRequest(result.ErrorResponse());
    }

    /// <summary>
    /// Ends a game
    /// </summary>
    /// <remarks>
    /// Deletes all data related to a game, including the board and its cells.
    /// </remarks>
    /// <param name="boardId">The unique identifier of the board.</param>
    /// <response code="200">The total number of games successfully ended by this request.</response>
    /// <response code="400">The request to end the game is invalid.</response>
    /// <response code="500">An internal error occurred.</response>
    [HttpDelete("{boardId}")]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Clear(string boardId)
    {
        var result = await _service.EndGame(boardId);

        if (result.IsSuccess)
        {
            return Ok(result.Model);
        }

        return BadRequest(result.ErrorResponse());
    }

}
